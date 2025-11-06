using System.Net.Http.Json;
using TaskGrid_App.Models;

namespace TaskGrid_App.Views;

[QueryProperty(nameof(SelectedTaskId), "taskId")]
public partial class TaskDetailsView : ContentPage
{
    private int _taskId;

    public int SelectedTaskId
    {
        get => _taskId;
        set
        {
            _taskId = value;
            OnPropertyChanged();
            LoadAndPopulateData();
        }
    }

    public TaskDetailsView()
	{
		InitializeComponent();
	}

    private async void LoadAndPopulateData()
    {
        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/TaskDetails", new
            {
                TaskId = _taskId
            });

            if (response.IsSuccessStatusCode)
            {
                TaskDetails? task = await response.Content.ReadFromJsonAsync<TaskDetails>();

                if (task == null)
                {
                    titleLabel.Text = "Response from server OK but task == null";
                    return;
                }

                PopulateViewWith(task);
            }
            else
            {
                titleLabel.Text = "Response form server NOT OK";
            }

        }
        catch (Exception ex)
        {
            titleLabel.Text = ex.Message;
        }
    }



    private void PopulateViewWith(TaskDetails data)
    {
        taskNameLabel.Text = data.TaskName;

        if (data.TaskDescription != null)
            taskDescriptionLabel.Text = data.TaskDescription;
        
        projectNameLabel.Text = data.ProjectName;
        
        if (data.AssignedToUser != null)
            assignedToLabel.Text = data.AssignedToUser;

        completeButton.IsVisible = data.AssignedToYou;
    }

    private async void OnClicked_CompleteTask(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(ConfirmTaskCompletionView)}?taskId={_taskId}");
    }
}