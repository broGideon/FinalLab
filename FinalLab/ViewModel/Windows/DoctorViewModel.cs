using System.Collections.ObjectModel;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Controls;
using System.Windows.Documents;
using FinalLab.Model;
using FinalLab.View.Cards;
using SecondLibPractice;

namespace FinalLab.ViewModel;

public class DoctorViewModel : BindingHelper
{
    #region MyRegion
    
    public FlowDocument AnalyzeRTB = new();

    public FlowDocument ResearchRTB = new();
    
    private ObservableCollection<ClientsView> _appointments = new ObservableCollection<ClientsView>();

    public ObservableCollection<ClientsView> Appointments
    {
        get => _appointments;
        set => SetField(ref _appointments, value);
    }

    private List<Speciality>? _specialities;

    public List<Speciality>? Specialities
    {
        get => _specialities;
        set => SetField(ref _specialities, value);
    }

    private Speciality _selectSpeciality;

    public Speciality SelectSpeciality
    {
        get => _selectSpeciality;
        set => SetField(ref _selectSpeciality, value);
    }

    private ObservableCollection<ReferralView?> _directions = new ObservableCollection<ReferralView?>();

    public ObservableCollection<ReferralView?> Directions
    {
        get => _directions;
        set => SetField(ref _directions, value);
    }

    private int _idDoctor;

    private string _complaints;

    public string Complaints
    {
        get => _complaints;
        set => SetField(ref _complaints, value);
    }

    private string _osmotor;

    public string Osmotor
    {
        get => _osmotor;
        set => SetField(ref _osmotor, value);
    }

    private string _primaryDiagnosis;

    public string PrimaryDiagnosis
    {
        get => _primaryDiagnosis;
        set => SetField(ref _primaryDiagnosis, value);
    }

    private string _secondaryDiagnosis;

    public string SecondaryDiagnosis
    {
        get => _secondaryDiagnosis;
        set => SetField(ref _secondaryDiagnosis, value);
    }

    private string _recommendations;

    public string Recommendations
    {
        get => _recommendations;
        set => SetField(ref _recommendations, value);
    }

    private string _patient;

    public string Patient
    {
        get => _patient;
        set => SetField(ref _patient, $"Пациент: {value}");
    }

    private string _user;

    public string User
    {
        get => _user;
        set => SetField(ref _user, value);
    }

    public DoctorViewModel(int idDoctor)
    {
        _idDoctor = idDoctor;
        Specialities = ApiHelper.Get<List<Speciality>>("Specialities");
    }
    #endregion

    public void AddDirections()
    {
        if (SelectSpeciality != null)
        {
            ReferralView referralView = new ReferralView((int)SelectSpeciality.IdSpeciality!, SelectSpeciality.NameSpecialities);
            referralView.DeleteSpeciality += (sender, args) => DeleteSpeciality(sender, args);
            Directions.Add(referralView);
        }
    }

    private void DeleteSpeciality(object sender, EventArgs args)
    {
        Directions.Remove(sender as ReferralView);
    }

    public void SelectedDate(object? sender, SelectionChangedEventArgs e)
    {
        DateOnly currentDate = DateOnly.FromDateTime((DateTime)(sender as DatePicker).SelectedDate);
        List<Appointment>? listAppointments = ApiHelper.Get<List<Appointment>>("Appointments");
        Appointments.Clear();
        foreach (var item in listAppointments)
        {
            if (item.AppointmentDate == currentDate)
            {
                Patient patient = ApiHelper.Get<Patient>("Patients", (long)item.Oms);
                Appointments.Add(new ClientsView($"{patient.Surname} {patient.FirstName} {patient.Patronymic}", item.AppointmentTime.ToString(), (long)item.Oms));
            }
        }
    }

    public void AddDopFile()
    {
        
    }

    public void Cancel()
    {
        
    }
}