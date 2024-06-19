using System.Windows;
using FinalLab.ViewModel.Windows;

namespace FinalLab.View.Pages;

public partial class ProfilePage
{
    public ProfilePage(PatientViewModel viewModel)
    {
        InitializeComponent();
        Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault()!.WindowTextBlock.Text = "Профиль";
        DataContext = viewModel;
    }

    private void AddAccount(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        var mainWindow = new MainWindow(true);
        mainWindow.Show();
        window.Close();
    }
}