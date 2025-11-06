using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskGrid_App.Helpers;
using TaskGrid_App.Models;

namespace TaskGrid_App.Views;

public partial class ProjectDetailsView : ContentPage
{
    private List<string> projectSeparatorsInOrder = new List<string>();
    private List<string> projectHeadersInOrder = new List<string>();
	public ProjectDetailsView()
	{
		InitializeComponent();
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        ownerOptionsLayout.IsVisible = false;

        if (StaticAppValues.SelectedProjectId == -1)
        {
            testLabel.Text = "Please first select a project.";
            return;
        }

        testLabel.Text = $"You selected project id: {StaticAppValues.SelectedProjectId}";


        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        try
        {
            var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/ProjectDetails", new
            {
                ProjectId = StaticAppValues.SelectedProjectId
            });

            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine(await response.Content.ReadAsStringAsync());
                AllProjectDetails? allProjectDetails = await response.Content.ReadFromJsonAsync<AllProjectDetails>();
                if (allProjectDetails == null)
                {
                    projectNameLabel.Text = "Server response OK but all project details == null";
                    return;
                }

                projectHeadersInOrder = allProjectDetails.GridHeaders;
                projectSeparatorsInOrder = allProjectDetails.GridSeparators;

                PrintProjectDetails(allProjectDetails);
            }
            else
            {
                Trace.WriteLine(await response.Content.ReadAsStringAsync());
                projectNameLabel.Text = "Server response NOT OK";
            }
        }
        catch (Exception ex)
        {
            projectNameLabel.Text = ex.Message;
        }        
    }

    private void PrintProjectDetails(AllProjectDetails details)
    {
        ownerOptionsLayout.IsVisible = details.IsProjectOwner;

        projectGridHolder.Children.Clear();
        projectMembersLayout.Children.Clear();

        projectNameLabel.Text = details.ProjectName;
        projectOwnerNameLabel.Text = details.ProjectOwner;

        if(details.ProjectMembers.Count > 0) 
        { 
            foreach(var member in details.ProjectMembers)
            {
                var memberEmailLabel = new Label();
                memberEmailLabel.Text = member;

                projectMembersLayout.Children.Add(memberEmailLabel);
            }
        }



        var separators = details.GridSeparators;
        var headers = details.GridHeaders;

        if (separators.Count == 0)
        {
            addHeaderButton.IsVisible = false;
            addTaskButton.IsVisible = false;

            var infoLabel = new Label();
            infoLabel.Text = "Please create the project grid first. There is nothing to display as there are no separators.";
            infoLabel.Margin = new Thickness(0, 20, 0, 20);
            infoLabel.HorizontalOptions = LayoutOptions.Center;
            projectGridHolder.Children.Add(infoLabel);
            return;
        }

        addHeaderButton.IsVisible = true;

        int separatorIndex = 0;
        foreach (var separator in separators)
        {
            var separatorLabel = new Label();
            separatorLabel.Text = separator;
            separatorLabel.Margin = new Thickness(50, 50, 0, 5);
            separatorLabel.FontAttributes = FontAttributes.Bold;
            separatorLabel.FontSize = 20;

            projectGridHolder.Children.Add(separatorLabel);

            if (headers.Count == 0)
            {
                addTaskButton.IsVisible = false;

                var infoLabel = new Label();
                infoLabel.Text = "Please create the project grid first. There is nothing to display as there are no headers.";
                infoLabel.Margin = new Thickness(0, 20, 0, 20);
                infoLabel.HorizontalOptions = LayoutOptions.Center;
                projectGridHolder.Children.Add(infoLabel);
                separatorIndex++;
                continue;
            }

            addTaskButton.IsVisible = true;
            var gridBorder = new Border
            {
                Stroke = Color.FromArgb("#000000"),
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) }
            };

            var separatorGrid = new Grid();
            separatorGrid.BackgroundColor = new Color(40, 40, 40);
            separatorGrid.Margin = new Thickness(0);

            separatorGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var tasksInEachHeader = new List<int>();
            for (int i = 0; i < headers.Count; i++)
            {
                separatorGrid.AddColumnDefinition(new ColumnDefinition());

                var headerLabel = new Label();
                headerLabel.Text = headers[i];
                headerLabel.HorizontalOptions = LayoutOptions.Center;

                separatorGrid.Add(headerLabel, i, 0);
                tasksInEachHeader.Add(0);
            }

            var tasks = details.ProjectTasks.Where(pt => pt.SeparatorIndex == separatorIndex).ToList();

            gridBorder.Content = separatorGrid;

            if (tasks.Count == 0)
            {
                separatorIndex++;
                projectGridHolder.Children.Add(gridBorder);
                continue;
            }

            var numberOfRows = 1;
            foreach (var task in tasks)
            {
                int taskId = task.TaskId;

                var taskBorder = new Border
                {
                    Stroke = Color.FromArgb("#ffffff"),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) }
                };

                var taskCard = new VerticalStackLayout();
                taskCard.BackgroundColor = new Color(50, 50, 50);
                taskCard.Padding = new Thickness(20);

                var taskNameLabel = new Label();
                taskNameLabel.Text = task.TaskName;
                taskNameLabel.FontAttributes = FontAttributes.Bold;
                taskNameLabel.HorizontalOptions = LayoutOptions.Center;
                taskNameLabel.Margin = new Thickness(0, 0, 0, 10);

                taskCard.Children.Add(taskNameLabel);


                var taskAssignedToLabel = new Label();
                taskAssignedToLabel.Text = "Assigned to:";
                taskAssignedToLabel.HorizontalOptions = LayoutOptions.Center;

                taskCard.Children.Add(taskAssignedToLabel);

                var assignedToNameLabel = new Label();
                assignedToNameLabel.Text = string.IsNullOrEmpty(task.AssignedToUser) ? "Nobody" : task.AssignedToUser;
                assignedToNameLabel.HorizontalOptions = LayoutOptions.Center;

                taskCard.Children.Add(assignedToNameLabel);

                var actionButtonsLayout = new FlexLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Direction = Microsoft.Maui.Layouts.FlexDirection.Row,
                    Wrap = Microsoft.Maui.Layouts.FlexWrap.Wrap,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                var taskDetailsButton = new Button();
                taskDetailsButton.Text = "View";
                taskDetailsButton.Margin = new Thickness(2);
                taskDetailsButton.Padding = new Thickness(2);
                taskDetailsButton.MinimumWidthRequest = 100;
                taskDetailsButton.Clicked += async (s, e) =>
                {
                    await Shell.Current.GoToAsync($"{nameof(TaskDetailsView)}?taskId={taskId}");
                };

                actionButtonsLayout.Children.Add(taskDetailsButton);

                if (string.IsNullOrEmpty(task.AssignedToUser))
                {
                    var assignToMeButton = new Button();
                    assignToMeButton.Text = "Take";
                    assignToMeButton.Margin = new Thickness(2);
                    assignToMeButton.Padding = new Thickness(2);
                    assignToMeButton.MinimumWidthRequest = 100;

                    assignToMeButton.Clicked += async (s, e) =>
                    {
                        await Shell.Current.GoToAsync($"{nameof(ConfirmTaskAssignmentView)}?taskId={taskId}");
                    };

                    actionButtonsLayout.Children.Add(assignToMeButton);
                }

                taskCard.Children.Add(actionButtonsLayout);

                var column = task.HeaderIndex;
                var row = tasksInEachHeader[column] + 1;

                if (numberOfRows <= row)
                {
                    separatorGrid.AddRowDefinition(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    numberOfRows++;
                }

                taskBorder.Content = taskCard;

                separatorGrid.Add(taskBorder, column, row);
                tasksInEachHeader[column]++;
            }


            projectGridHolder.Children.Add(gridBorder);
            separatorIndex++;
        }

    }

    private async void OnClicked_AddSeparator(object sender, EventArgs e)
    {
        StaticAppValues.NamesInOrder = new List<string>(projectSeparatorsInOrder);
        await Shell.Current.GoToAsync(nameof(AddSeparator));
    }

    private async void OnClicked_AddHeader(object sender, EventArgs e)
    {
        StaticAppValues.NamesInOrder = new List<string>(projectHeadersInOrder);
        await Shell.Current.GoToAsync(nameof(AddHeader));
    }

    private async void OnClicked_AddTask(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddTaskView));
    }

    private async void OnClicked_AddMember(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddProjectMemberView));
    }
}