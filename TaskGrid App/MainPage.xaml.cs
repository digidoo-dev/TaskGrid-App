using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.Net.Http.Json;
using TaskGrid_App.Helpers;
using TaskGrid_App.Models;

namespace TaskGrid_App;

public partial class MainPage : ContentPage
{
    private Label? infoLabel;
    private List<Border> displayedProjectsList = new List<Border>();

    public MainPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.GetAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/Projects");
            if (response.IsSuccessStatusCode)
            {
                Trace.WriteLine(await response.Content.ReadAsStringAsync());
                ProjectList? allProjects = await response.Content.ReadFromJsonAsync<ProjectList>();
                
                PrintAllProjects(allProjects);
            }
            else
            {
                titleLabel.Text = "Response from server NOT OK";
            }
        }
        catch (Exception ex)
        {
            titleLabel.Text = ex.Message;
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (infoLabel != null)
        {
            projectsFlexContainer.Children.Remove(infoLabel);
        }
        if (displayedProjectsList.Count > 0)
        {
            foreach (var project in displayedProjectsList)
            {
                projectsFlexContainer.Children.Remove(project);
            }
            displayedProjectsList.Clear();
        }
    }



    private void PrintAllProjects(ProjectList? projectsList)
    {
        
        if (projectsList == null || projectsList.ProjectIds.Count == 0 || projectsList.ProjectNames.Count == 0)
        {
            Trace.WriteLine($"Print All Projects ==null || ==0");
            infoLabel = new Label();
            infoLabel.Text = "You don't have any projects.";
            projectsFlexContainer.Children.Add(infoLabel);
        }
        else
        {
            for (int i = 0; i < projectsList.ProjectIds.Count; i++)
            {
                var projectBorder = new Border();
                projectBorder.Stroke = new Color(220, 220, 220);
                projectBorder.StrokeThickness = 4;
                projectBorder.BackgroundColor = new Color(50, 50, 50);
                projectBorder.Padding = new Thickness(10);
                projectBorder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) };
                projectBorder.Margin = new Thickness(10);

                var projectContainer = new VerticalStackLayout();
                

                var projectNameLabel = new Label();
                var enterProjectButton = new Button();

                projectNameLabel.Text = projectsList.ProjectNames[i];
                projectNameLabel.FontAttributes = FontAttributes.Bold;
                projectNameLabel.HorizontalOptions = LayoutOptions.Center;
                projectNameLabel.Margin = new Thickness(0, 0, 0, 10);

                enterProjectButton.Text = "View Project Board";

                var projectId = projectsList.ProjectIds[i];
                enterProjectButton.Clicked += async (s, e) =>
                {
                    StaticAppValues.SelectedProjectId = projectId;
                    await Shell.Current.GoToAsync($"///{nameof(Views.ProjectDetailsView)}");
                };

                projectContainer.Children.Add(projectNameLabel);
                projectContainer.Children.Add(enterProjectButton);

                projectBorder.Content = projectContainer;

                projectsFlexContainer.Children.Add(projectBorder);
                displayedProjectsList.Add(projectBorder);
            }
        }
    }
}
