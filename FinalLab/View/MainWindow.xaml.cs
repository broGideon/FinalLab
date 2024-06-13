using System.Windows;
using System.Windows.Input;
using FinalLab.Model;
using FinalLab.View;
using FinalLab.View.Pages;
using FinalLab.ViewModel;
using Spire.Pdf.Exporting.XPS.Schema;
using Wpf.Ui.Controls;
namespace FinalLab;

public partial class MainWindow : Window
{
    private MainViewModel _viewModel;
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
        PageFrame.Content = new AuthorizationClientPage(_viewModel);
        _viewModel.OpenAdminWindow += (_, _) => OpenAdmin();
        _viewModel.OpenClientWindow += (_, _) => OpenPatient();
        _viewModel.OpenDoctorWindow += (_, _) => OpenDoctor();
        _viewModel.SwitchPage += (_, _) => SwithPage();
        
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

    private void OpenPatient()
    {
        PatientWindow window = new PatientWindow();
        window.Show();
        Close();
    }
    
    private void OpenAdmin()
    {
        AdministratorWindow window = new AdministratorWindow();
        window.Show();
        Close();
    }
    
    private void OpenDoctor()
    {
        DoctorWindow window = new DoctorWindow();
        window.Show();
        Close();
    }

    private void SwithPage()
    {
        if (PageFrame.Content.GetType() == typeof(AuthorizationClientPage))
            PageFrame.Content = new AuthorizationDoctorPage(_viewModel);
        else
            PageFrame.Content = new AuthorizationClientPage(_viewModel);
    }
}