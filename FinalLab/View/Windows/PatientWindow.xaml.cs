using System.Windows;
using System.Windows.Input;
using FinalLab.View.Pages;
using FinalLab.ViewModel;

namespace FinalLab.View;

public partial class PatientWindow : Window
{
    private PatientViewModel _viewModel;
    
    public PatientWindow()
    {
        InitializeComponent();
        _viewModel = new PatientViewModel();
        DataContext = _viewModel;
        PageFrame.Content = new HomePatientPage(_viewModel);
    }

    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        PageFrame.Content = new ProfilePage(_viewModel);
    }

    private void OpenHomePatient(object sender, MouseButtonEventArgs e)
    {
        PageFrame.Content = new HomePatientPage(_viewModel);
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
}