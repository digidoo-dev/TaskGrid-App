using TaskGrid_App.Views;

namespace TaskGrid_App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddProjectMemberView), typeof(AddProjectMemberView));
            Routing.RegisterRoute(nameof(AddSeparator), typeof(AddSeparator));
            Routing.RegisterRoute(nameof(AddHeader), typeof(AddHeader));
            Routing.RegisterRoute(nameof(AddTaskView), typeof(AddTaskView));
            Routing.RegisterRoute(nameof(ConfirmTaskAssignmentView), typeof(ConfirmTaskAssignmentView));
            Routing.RegisterRoute(nameof(TaskDetailsView), typeof(TaskDetailsView));
            Routing.RegisterRoute(nameof(ConfirmTaskCompletionView), typeof(ConfirmTaskCompletionView));
        }
    }
}
