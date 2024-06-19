using System.Windows;
using FinalLab.ViewModel.Pages;

namespace FinalLab.View.Pages;

public partial class ChoosingDoctorPage
{
    public ChoosingDoctorPage(int idSpeciality, int idDoctor = -1, int idAppointment = -1)
    {
        InitializeComponent();
        DataContext = new ChoosingDoctorViewModel(idSpeciality, idDoctor, idAppointment);
    }

    private void Back(object sender, RoutedEventArgs e)
    {
        Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault()!.PageFrame.GoBack();
    }
}