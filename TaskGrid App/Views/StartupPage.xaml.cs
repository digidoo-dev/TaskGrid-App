namespace TaskGrid_App.Views;

public partial class StartupPage : ContentPage
{
	public StartupPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var token = await SecureStorage.GetAsync("auth_token");

        await Task.Delay(1500);

        if (!string.IsNullOrEmpty(token))
        {
            Application.Current!.Windows[0].Page = new AppShell();
        }
        else
        {
            Application.Current!.Windows[0].Page = new LoginOrRegisterView();
        }
    }
}