using System.Net.Http.Json;
using TaskGrid_App.Helpers;

namespace TaskGrid_App.Views;

public partial class AddSeparator : ContentPage
{
    private List<string> existingSeparators = new List<string>();

    public AddSeparator()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        existingSeparators = StaticAppValues.NamesInOrder;

        if (existingSeparators.Count == 0)
        {
            afterBeforePicker.IsVisible = false;
            optionsPicker.IsVisible = false;
        }
        else
        {
            afterBeforePicker.IsVisible = true;
            afterBeforePicker.ItemsSource = new List<string>() { "After", "Before" };
            afterBeforePicker.SelectedIndex = 0;

            optionsPicker.IsVisible = true;
            optionsPicker.ItemsSource = existingSeparators;
            optionsPicker.SelectedIndex = existingSeparators.Count - 1;
        }
    }

    private async void OnClicked_AddSeparator(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameEntry.Text))
            return;

        confirmButton.IsEnabled = false;

        int projectId = StaticAppValues.SelectedProjectId;
        string name = nameEntry.Text;
        int orderIndex;

        if (existingSeparators.Count == 0)
        {
            orderIndex = 0;
        }
        else
        {
            if (afterBeforePicker.SelectedIndex == 0)
            {
                orderIndex = optionsPicker.SelectedIndex + 1;
            }
            else
            {
                orderIndex = optionsPicker.SelectedIndex;
            }
        }

        var token = await SecureStorage.GetAsync("auth_token");
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.PostAsJsonAsync("https://taskgrid-ghh2c3c6cwf0breu.polandcentral-01.azurewebsites.net/API/AddGridSeparator", new
            {
                ProjectId = projectId,
                SeparatorName = name,
                SeparatorOrderIndex = orderIndex
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
        }
    }
}