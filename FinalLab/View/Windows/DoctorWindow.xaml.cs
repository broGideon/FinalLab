using System.Windows;
using System.Windows.Input;
using FinalLab.Properties;
using FinalLab.View.Windows;
using FinalLab.ViewModel;
using Wpf.Ui.Controls;

namespace FinalLab.View;

public partial class DoctorWindow : Window
{
    private DoctorViewModel _viewModel;

    public DoctorWindow(int id)
    {
        InitializeComponent();
        _viewModel = new DoctorViewModel(id);
        DataContext = _viewModel;
        PageFrame.Content = new DortorPage(_viewModel);
        if (App.Theme == "Dark")
            IconTheme.Symbol = SymbolRegular.WeatherSunny32;
        else
            IconTheme.Symbol = SymbolRegular.WeatherMoon28;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
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
        {
            App.Theme = "Light";
            IconTheme.Symbol = SymbolRegular.WeatherMoon28;
        }
        else
        {
            App.Theme = "Dark";
            IconTheme.Symbol = SymbolRegular.WeatherSunny32;
        }
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Settings.Default.CurrentDoctor = -1;
        Settings.Default.Save();
        MainWindow window = new MainWindow();
        window.Show();
        Close();
    }
}