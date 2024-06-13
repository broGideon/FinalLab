using System.Windows;
using System.Windows.Input;
using FinalLab.ViewModel;

namespace FinalLab.View;

public partial class DoctorWindow : Window
{
    private DoctorViewModel _viewModel;

    public DoctorWindow()
    {
        InitializeComponent();
        _viewModel = new DoctorViewModel();
        DataContext = _viewModel;
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

    private void SwitchTheme(object sender, RoutedEventArgs e)
    {
        if (App.Theme == "Dark")
            App.Theme = "Light";
        else
            App.Theme = "Dark";
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        MainWindow window = new MainWindow();
        window.Show();
        Close();
    }
}