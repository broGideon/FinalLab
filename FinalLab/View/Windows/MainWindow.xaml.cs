using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using FinalLab.Properties;
using FinalLab.View;
using FinalLab.View.Pages;
using FinalLab.ViewModel;

namespace FinalLab;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow() : this(false)
    {
    }

    public MainWindow(bool addAccount = false)
    {
        InitializeComponent();
        if (!string.IsNullOrEmpty(Settings.Default.CurrentUsers) && !addAccount)
        {
            OpenPatient();
            return;
        }

        if (Settings.Default.CurrentDoctor != -1 && !addAccount)
        {
            OpenDoctor();
            return;
        }

        if (Settings.Default.CurrentAdmin != -1 && !addAccount)
        {
            OpenAdmin();
            return;
        }

        _viewModel = new MainViewModel(addAccount);
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
        if (e.LeftButton == MouseButtonState.Pressed) DragMove();
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
        var window = new PatientWindow();
        window.Show();
        Close();
    }

    private void OpenAdmin()
    {
        var window = new AdministratorWindow();
        window.Show();
        Close();
    }

    private void OpenDoctor()
    {
        var window = new DoctorWindow(Settings.Default.CurrentDoctor);
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