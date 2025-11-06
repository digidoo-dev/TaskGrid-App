using TaskGrid_App.Views;

namespace TaskGrid_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Application.Current!.UserAppTheme = AppTheme.Dark;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new StartupPage());
        }
    }
}