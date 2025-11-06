using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskGrid_App.Helpers;
using TaskGrid_App.Models;

namespace TaskGrid_App.Views;

public partial class AddTaskView : ContentPage
{
    private TaskCreationInfo? taskCreationInfo;



	public AddTaskView()
	{
		InitializeComponent();
	}


    protected async override void OnAppearing()
    {
        base.OnAppearing();

        confirmButton.IsEnabled = false;
        taskNameEntry.Text = "";
        taskDescriptionEntry.Text = "";

        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/TaskCreationInfo", new
            {
                ProjectId = StaticAppValues.SelectedProjectId
            });

            if (response.IsSuccessStatusCode)
            {
                taskCreationInfo = await response.Content.ReadFromJsonAsync<TaskCreationInfo>();
                
                if (taskCreationInfo == null)
                {
                    titleLabel.Text = "Initial Response from server OK, but TaskCreationInfo is null";
                    return;
                }


                separatorPicker.ItemsSource = taskCreationInfo.SeparatorNames;
                separatorPicker.SelectedIndex = 0;


                headerPicker.ItemsSource = taskCreationInfo.HeaderNames;
                headerPicker.SelectedIndex = 0;

                confirmButton.IsEnabled = true;
            }
            else
            {
                titleLabel.Text = "Initial Response from server NOT OK";
            }
        }
        catch (Exception ex)
        {
            titleLabel.Text = ex.Message;
        }

    }


    private async void OnClicked_AddTask(object sender, EventArgs e)
    {
		if (string.IsNullOrWhiteSpace(taskNameEntry.Text))
			return;
        if (taskCreationInfo == null)
            return;


        if (taskCreationInfo.HeaderNames.Count == 0 || taskCreationInfo.SeparatorNames.Count == 0)
            return;

        confirmButton.IsEnabled = false;

        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/AddTask", new
            {
                ProjectId = StaticAppValues.SelectedProjectId,
                TaskName = taskNameEntry.Text,
                TaskDescription = taskDescriptionEntry.Text,
                SeparatorProjectGridId = taskCreationInfo.SeparatorProjectGridIds[separatorPicker.SelectedIndex],
                HeaderProjectGridId = taskCreationInfo.HeaderProjectGridIds[headerPicker.SelectedIndex]
            });

            if (response.IsSuccessStatusCode)
            {
                confirmButton.IsEnabled = true;
                await Shell.Current.GoToAsync($"///{nameof(ProjectDetailsView)}");
            }
            else
            {
                titleLabel.Text = "Response from server NOT OK";
                confirmButton.IsEnabled = true;
            }
        }
        catch (Exception ex)
        {
            titleLabel.Text = ex.Message;
            confirmButton.IsEnabled = true;
        }

    }
}