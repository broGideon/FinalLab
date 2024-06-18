using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FinalLab.Properties;
using FinalLab.ViewModel;
using FinalLab.ViewModel.Windows;

namespace FinalLab.View.Pages;

public partial class ProfilePage
{
    public ProfilePage(PatientViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void CancelAccount(object sender, RoutedEventArgs e)
    {
        //var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        MainWindow mainWindow = new MainWindow();
        Settings.Default.CurrentUsers = string.Empty;
        Settings.Default.Save();
        //mainWindow.Show();
        //window.Close();
    }
}