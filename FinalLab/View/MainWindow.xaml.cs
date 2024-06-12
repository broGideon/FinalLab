using System.Windows;
using System.Windows.Input;
using FinalLab.Model;
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
        List<Admin> aa = ApiHelper.Get<List<Admin>>("Admins", 3);
        
        Console.WriteLine(aa[0].Patronymic);
        /*foreach (var item in aa)
        {
            Console.WriteLine(item.FirstName);
        }*/
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