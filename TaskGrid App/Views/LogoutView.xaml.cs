using System.Threading.Tasks;

namespace TaskGrid_App.Views;

public partial class LogoutView : ContentPage
{
	public LogoutView()
	{
		InitializeComponent();
	}

    private void OnClicked_LogOut(object sender, EventArgs e)
    {
		SecureStorage.Remove("auth_token");
		Application.Current!.Windows[0].Page = new LoginOrRegisterView();
    }
}