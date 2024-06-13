using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FinalLab.View.Pages;
using FinalLab.ViewModel;

namespace FinalLab.View;

public partial class AdministratorWindow : Window
{
    private AdministratorViewModel _viewModel;
    public AdministratorWindow()
    {
        InitializeComponent();
        _viewModel = new AdministratorViewModel();
        DataContext = _viewModel;
        PageFrame.Content = new UserForm(_viewModel);
        _viewModel.SwitchForm += (_, _) => SwitchRole();
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

    private void SwitchRole()
    {
        if (RoleComboBox.SelectedItem == "Пользователь")
            PageFrame.Content = new UserForm(_viewModel);
        else if (RoleComboBox.SelectedItem == "Доктор")
            PageFrame.Content = new DoctorForm(_viewModel);
        else
            PageFrame.Content = new AdminForm(_viewModel);
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        MainWindow window = new MainWindow();
        Close();
    }

    private void SwitchTheme(object sender, RoutedEventArgs e)
    {
        if (App.Theme == "Light")
            App.Theme = "Dark";
        else
            App.Theme = "Light";
    }
}