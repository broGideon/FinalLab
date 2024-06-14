using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
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
        _viewModel.SwitchPage += (_, _) => BeginAnimation();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
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
        DoctorWindow window = new DoctorWindow(Convert.ToInt32(_viewModel.Login));
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
    
    private void BeginAnimation()
    {
        var opacityAnim = new DoubleAnimation();
        opacityAnim.From = 0;
        opacityAnim.To = 1;
        opacityAnim.Duration = TimeSpan.FromSeconds(0.5);
        PageFrame.BeginAnimation(OpacityProperty, opacityAnim);
    }

}