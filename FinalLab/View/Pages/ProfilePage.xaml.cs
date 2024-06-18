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
        viewModel.Close += (_, _) => Close();
    }

    private void Close()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        window.Close();
    }

    private void AddAccount(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        MainWindow mainWindow = new MainWindow(true);
        mainWindow.Show();
        window.Close();
    }
}