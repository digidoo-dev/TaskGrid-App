using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskGrid_App.Helpers;

namespace TaskGrid_App.Views;

public partial class AddProjectMemberView : ContentPage
{
	public AddProjectMemberView()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        titleLabel.Text = "Add Project Member";
        emailEntry.Text = "";
        roleEntry.Text = "";
        addMemberButton.IsEnabled = true;
    }

    private async void OnClicked_AddMember(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(emailEntry.Text))
            return;

        addMemberButton.IsEnabled = false;

        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/AddProjectMember", new
            {
                ProjectId = StaticAppValues.SelectedProjectId,
                NewMemberEmail = emailEntry.Text,
                NewMemberRole = roleEntry.Text
            });

            if (response.IsSuccessStatusCode)
            {
                titleLabel.Text = "Succesfully added project member. Add another:";
                emailEntry.Text = "";
                roleEntry.Text = "";
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

        addMemberButton.IsEnabled = true;
    }

}