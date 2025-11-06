using System.Net.Http.Json;
using TaskGrid_App.Models;

namespace TaskGrid_App.Views;

public partial class CreateNewProjectView : ContentPage
{
	public CreateNewProjectView()
	{
		InitializeComponent();
	}

    private async void OnClicked_CreateProject(object sender, EventArgs e)
    {
		if (string.IsNullOrWhiteSpace(projectNameEntry.Text)) return;

        projectNameEntry.IsEnabled = false;
        createProjectButton.IsEnabled = false;

        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/CreateProject", new
        {
            Name = projectNameEntry.Text
        });

        if (response.IsSuccessStatusCode)
        {
            projectNameEntry.IsEnabled = true;
            createProjectButton.IsEnabled = true;
            await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
        }
        else
        {
            titleLabel.Text = "Response from server NOT OK";
            projectNameEntry.IsEnabled = true;
            createProjectButton.IsEnabled = true;
        }
    }
}