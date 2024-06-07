using System.Windows;
using System.Windows.Input;
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
}