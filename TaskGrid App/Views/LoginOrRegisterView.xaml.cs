using System.Net.Http.Json;

namespace TaskGrid_App.Views;

public partial class LoginOrRegisterView : ContentPage
{
	public LoginOrRegisterView()
	{
		InitializeComponent();
	}

    private async void LoginAttempt(object sender, EventArgs e)
    {
		if (string.IsNullOrWhiteSpace(emailEntry.Text) || string.IsNullOrWhiteSpace(passwordEntry.Text))
			return;

		emailEntry.IsEnabled = false;
		passwordEntry.IsEnabled = false;

		var client = new HttpClient();
		var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/Login", new
		{
			Email = emailEntry.Text,
			Password = passwordEntry.Text
		});

		if (response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
			await SecureStorage.SetAsync("auth_token", result!.Token);
			Application.Current!.Windows[0].Page = new AppShell();
		}
		else
		{
			titleLabel.Text = "Response NOT success code.";
			emailEntry.IsEnabled = true;
			passwordEntry.IsEnabled = true;
		}

    }


    private async void OpenRegisterWebsite(object sender, EventArgs e)
    {
		try
		{
			Uri uri = new Uri("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/Identity/Account/Register");
			await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
		}
		catch (Exception ex)
		{
			titleLabel.Text = ex.Message;
		}
    }


    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }

}