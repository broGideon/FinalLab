using System.Collections.ObjectModel;
using System.Windows.Data;
using FinalLab.Model;
using FinalLab.View.Cards;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class HomePatientViewModel : BindingHelper
{
    #region MyRegion

    private ObservableCollection<SpecialtyDoctor> _specialtyDoctorCards = new();

    public ObservableCollection<SpecialtyDoctor> SpecialtyDoctorCards
    {
        get => _specialtyDoctorCards;
        set => SetField(ref _specialtyDoctorCards, value);
    }

    private ObservableCollection<Data> _currentRecords = new();
    
    public ObservableCollection<Data> CurrentRecords
    {
        get => _currentRecords;
        set => SetField(ref _currentRecords, value);
    }
    
    private ObservableCollection<Data> _archivedRecords = new();
    
    public ObservableCollection<Data> ArchivedRecords
    {
        get => _archivedRecords;
        set => SetField(ref _archivedRecords, value);
    }
    

    #endregion
    public HomePatientViewModel()
    {
        _ = LoadSpecialities();
        _ = LoadAppointments();
    }

    private async Task LoadSpecialities()
    {
        List<Speciality>? specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        foreach (var speciality in specialities!)
        {
            SpecialtyDoctor specialtyDoctor = new SpecialtyDoctor(speciality.NumberImage.ToString(), speciality.NameSpecialities);
            SpecialtyDoctorCards.Add(specialtyDoctor);
        }
    }

    private async Task LoadAppointments()
    {
        List<Appointment>? appointments = ApiHelper.Get<List<Appointment>>("Appointments");
        List<Appointments> monthAppointments = new();
        List<RecordsArchive> recordsArchives = new();
        int month = appointments[0].AppointmentDate.Month;
        foreach (var appointment in appointments!)
        {
            Doctor? doctor = ApiHelper.Get<Doctor>("Doctors", (long)appointment.DoctorId!);
            string speciality = ApiHelper.Get<Speciality>("Specialities", doctor!.IdDoctor)!.NameSpecialities;
            Patient patient = ApiHelper.Get<Patient>("Patients", (long)appointment.Oms!)!;
            if (month == appointment.AppointmentDate.Month)
            {
                if (appointment.StatusId != 4)
                {
                   var elem = new Appointments(speciality, $"{patient.Surname} {patient.FirstName} {patient.Patronymic}", appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, doctor.IdDoctor, (int)appointment.IdAppointment!);
                   monthAppointments.Add(elem);
                   elem.Delete += (sender, args) => Delete(sender, args);
                   elem.Move += (sender, args) => Move(sender, args);
                }
                else
                {
                    var elem = new RecordsArchive(speciality, $"{patient.Surname} {patient.FirstName} {patient.Patronymic}", appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, doctor.IdDoctor, (int)appointment.IdAppointment!);
                    recordsArchives.Add(elem);
                    elem.Delete += (sender, args) => Delete(sender, args);
                    elem.Repeat += (sender, args) => Repeat(sender, args);
                }
            }
            else if (month != appointment.AppointmentDate.Month && appointments.Count-1 != appointments.IndexOf(appointment))
            {
                month = appointment.AppointmentDate.Month;
                if (monthAppointments.Count != 0)
                {
                    CurrentRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), monthAppointments.ToList()));
                    monthAppointments.Clear();
                }
                if (recordsArchives.Count != 0)
                {
                    ArchivedRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), recordsArchives.ToList()));
                    recordsArchives.Clear();
                }
                if (appointment.StatusId != 4)
                {
                    var elem = new Appointments(speciality, $"{patient.Surname} {patient.FirstName} {patient.Patronymic}", appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, doctor.IdDoctor, (int)appointment.IdAppointment!);
                    elem.Delete += (sender, args) => Delete(sender, args);
                    elem.Move += (sender, args) => Move(sender, args);
                }
                else
                {
                    var elem = new RecordsArchive(speciality, $"{patient.Surname} {patient.FirstName} {patient.Patronymic}", appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, doctor.IdDoctor, (int)appointment.IdAppointment!);
                    recordsArchives.Add(elem);
                    elem.Delete += (sender, args) => Delete(sender, args);
                    elem.Repeat += (sender, args) => Repeat(sender, args);
                }
            }
            if (appointments.Count - 1 == appointments.IndexOf(appointment))
            {
                if (monthAppointments.Count != 0) CurrentRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), monthAppointments.ToList()));
                if (recordsArchives.Count != 0) ArchivedRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), recordsArchives.ToList()));
            }
        }
    }

    private void Move(object sender, EventArgs args)
    {
        
    }
    
    private void Delete(object sender, EventArgs args)
    {
        int id;
        if (sender is Appointments)
            id = (sender as Appointments).IdAppointment;
        else
            id = (sender as RecordsArchive).IdAppointment;
        
        ApiHelper.Delete("AnalysDocuments", id);
        ApiHelper.Delete("ResearchDocuments", id);
        ApiHelper.Delete("AppointmentDocuments", id);
        ApiHelper.Delete("Appointments", id);
    }

    private void Repeat(object sender, EventArgs args)
    {
        
    }
}