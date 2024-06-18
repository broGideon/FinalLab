using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class ChoosingDoctorViewModel : BindingHelper
{
    public ChoosingDoctorViewModel(int idSpeciality, int idDoctor, int idAppointment)
    {
        Console.WriteLine($"idSpeciality: {idSpeciality}, idDoctor: {idDoctor}, idAppointment: {idAppointment}");
    }
}