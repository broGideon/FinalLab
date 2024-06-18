using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FinalLab.ViewModel;
using FinalLab.ViewModel.Pages;

namespace FinalLab.View.Pages;

public partial class MakeAppointmentPage : Page
{
    public MakeAppointmentPage()
    {
        InitializeComponent();
        DataContext = new MakeAppointmentViewModel();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        window.PageFrame.Content = new RecordPage();
    }
}