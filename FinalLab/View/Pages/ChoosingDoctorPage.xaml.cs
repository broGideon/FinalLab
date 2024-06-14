using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinalLab.View.Pages;

public partial class ChoosingDoctorPage : Page
{
    public ChoosingDoctorPage()
    {
        InitializeComponent();
    }
    
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        window.Close();
    }

    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
            window.DragMove();
        }
    }

    private void UnwrapButton_Click(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        if (window.WindowState == WindowState.Normal)
            window.WindowState = WindowState.Maximized;
        else
            window.WindowState = WindowState.Normal;
    }

    private void RollUpButton_Click(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        window.WindowState = WindowState.Minimized;
    }
}