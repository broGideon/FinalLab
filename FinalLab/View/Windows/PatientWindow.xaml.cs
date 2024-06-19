using System.Windows;
using System.Windows.Input;
using FinalLab.View.Pages;
using FinalLab.ViewModel.Windows;

namespace FinalLab.View;

public partial class PatientWindow : Window
{
    private readonly PatientViewModel _viewModel;

    public PatientWindow()
    {
        InitializeComponent();
        _viewModel = new PatientViewModel();
        _viewModel.SwitchUsers += (sender, args) => OpenHomePatient();
        _viewModel.Close += (_, _) => ExitUser();
        DataContext = _viewModel;
    }

    private void OpenHomePatient()
    {
        PageFrame.Content = null;
        WindowTextBlock.Text = string.Empty;
    }

    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        PageFrame.Content = new ProfilePage(_viewModel);
    }

    private void OpenMakeAppointmentPatient(object sender, EventArgs e)
    {
        PageFrame.Content = new MakeAppointmentPage();
    }

    private void OpenAppointments(object sender, MouseButtonEventArgs e)
    {
        PageFrame.Content = new AppointmentPage();
    }

    private void OpenAnalyzes(object sender, MouseButtonEventArgs e)
    {
        PageFrame.Content = new AnalyzePage();
    }

    private void OpenResearches(object sender, MouseButtonEventArgs e)
    {
        PageFrame.Content = new ResearchePage();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void UnwrapButton_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Normal)
            WindowState = WindowState.Maximized;
        else
            WindowState = WindowState.Normal;
    }

    private void RollUpButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void ExitUser()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        var mainWindow = new MainWindow();
        mainWindow.Show();
        window.Close();
    }
}