using System.Net.Http.Json;

namespace TaskGrid_App.Views;

[QueryProperty(nameof(SelectedTaskId), "taskId")]
public partial class ConfirmTaskAssignmentView : ContentPage
{
	private int _taskId;

	public int SelectedTaskId
	{
		get => _taskId;
		set
		{
			_taskId = value;
			OnPropertyChanged();
		}
	}

	public ConfirmTaskAssignmentView()
	{
		InitializeComponent();
	}

    private async void OnClicked_Confirm(object sender, EventArgs e)
    {
		confirmButton.IsEnabled = false;

        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        try
        {
            var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/AssignTask", new
            {
                TaskId = _taskId
            });

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"///{nameof(ProjectDetailsView)}");
            }
            else
            {
                titleLabel.Text = "Server response NOT OK";
            }
        }
        catch (Exception ex)
        {
            titleLabel.Text = ex.Message;
        }

        confirmButton.IsEnabled = true;
    }
}