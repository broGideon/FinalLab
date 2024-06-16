using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using FinalLab.Model;
using FinalLab.View.Cards;
using SecondLibPractice;
using Spire.Pdf.Exporting.XPS.Schema;

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

    private DateOnly _selectionDateCurrentFrom;
    private DateOnly _selectionDateCurrentTo;
    private DateOnly _selectionDateArchivesFrom;
    private DateOnly _selectionDateArchivesTo;

    public HomePatientViewModel()
    {
        _ = LoadSpecialities();
        _ = LoadCurrentAppointments();
        _ = LoadArchivesAppointments();
    }
    #endregion

    private async Task LoadSpecialities()
    {
        List<Speciality>? specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        foreach (var speciality in specialities!)
        {
            SpecialtyDoctor specialtyDoctor = new SpecialtyDoctor(speciality.NumberImage.ToString(), speciality.NameSpecialities);
            SpecialtyDoctorCards.Add(specialtyDoctor);
        }
    }

    private async Task LoadCurrentAppointments()
    {
        List<Appointment>? appointments = ApiHelper.Get<List<Appointment>>("Appointments");
        ObservableCollection<Appointments> monthAppointments = new();
        int month = appointments[0].AppointmentDate.Month;
        foreach (var appointment in appointments!)
        {
            if ((appointment.AppointmentDate > _selectionDateCurrentFrom &&
                appointment.AppointmentDate > _selectionDateCurrentTo) ||
                (_selectionDateCurrentFrom == DateOnly.MinValue && _selectionDateCurrentTo == DateOnly.MinValue))
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
                }
                else if (month != appointment.AppointmentDate.Month && appointments.Count-1 != appointments.IndexOf(appointment))
                {
                    month = appointment.AppointmentDate.Month;
                    CurrentRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), new ObservableCollection<Appointments>(monthAppointments)));
                    monthAppointments.Clear();
                    if (appointment.StatusId != 4)
                    {
                        var elem = new Appointments(speciality, $"{patient.Surname} {patient.FirstName} {patient.Patronymic}", appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, doctor.IdDoctor, (int)appointment.IdAppointment!);
                        monthAppointments.Add(elem);
                        elem.Delete += (sender, args) => Delete(sender, args);
                        elem.Move += (sender, args) => Move(sender, args);
                    }
                }
            }
            if (appointments.Count - 1 == appointments.IndexOf(appointment))
                if (monthAppointments.Count != 0) CurrentRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), new ObservableCollection<Appointments>(monthAppointments)));
        }
    }
    
     private async Task LoadArchivesAppointments()
    {
        List<Appointment>? appointments = ApiHelper.Get<List<Appointment>>("Appointments");
        ObservableCollection<RecordsArchive> recordsArchives = new();
        int month = appointments[0].AppointmentDate.Month;
        foreach (var appointment in appointments!)
        {
            if ((appointment.AppointmentDate > _selectionDateArchivesFrom &&
                appointment.AppointmentDate > _selectionDateArchivesTo) ||
                (_selectionDateArchivesFrom == DateOnly.MinValue && _selectionDateArchivesTo == DateOnly.MinValue))
            {
                Doctor? doctor = ApiHelper.Get<Doctor>("Doctors", (long)appointment.DoctorId!);
                string speciality = ApiHelper.Get<Speciality>("Specialities", doctor!.IdDoctor)!.NameSpecialities;
                Patient patient = ApiHelper.Get<Patient>("Patients", (long)appointment.Oms!)!;
                if (month == appointment.AppointmentDate.Month)
                {
                    if (appointment.StatusId == 4)
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
                    ArchivedRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), new ObservableCollection<RecordsArchive>(recordsArchives)));
                    recordsArchives.Clear();
                    if (appointment.StatusId == 4)
                    {
                        var elem = new RecordsArchive(speciality, $"{patient.Surname} {patient.FirstName} {patient.Patronymic}", appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, doctor.IdDoctor, (int)appointment.IdAppointment!);
                        recordsArchives.Add(elem);
                        elem.Delete += (sender, args) => Delete(sender, args);
                        elem.Repeat += (sender, args) => Repeat(sender, args);
                    } 
                }
            }
            if (appointments.Count - 1 == appointments.IndexOf(appointment))
            {
                if (recordsArchives.Count != 0) ArchivedRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"), new ObservableCollection<RecordsArchive>(recordsArchives)));
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
        {
            id = (sender as Appointments).IdAppointment;
            foreach (var item in CurrentRecords)
                if (item.ElementCurrents.Remove(sender as Appointments))
                    break;
        }
        else
        {
            id = (sender as RecordsArchive).IdAppointment;
            foreach (var item in ArchivedRecords)
                if (item.ElementArchives.Remove(sender as RecordsArchive))
                    break;
        }
        
        ApiHelper.Delete("AnalysDocuments", id);
        ApiHelper.Delete("ResearchDocuments", id);
        ApiHelper.Delete("AppointmentDocuments", id);
        ApiHelper.Delete("Appointments", id);
    }

    private void Repeat(object sender, EventArgs args)
    {
        
    }

    public void SelectedDateCurrentFrom(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateCurrentFrom = DateOnly.FromDateTime((DateTime)((sender as DatePicker)!).SelectedDate!);
        _ = LoadCurrentAppointments();
    }
    
    public void SelectedDateCurrentTo(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateCurrentTo = DateOnly.FromDateTime((DateTime)((sender as DatePicker)!).SelectedDate!);
        _ = LoadCurrentAppointments();
    }
    
    public void SelectedDateArchivesFrom(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateArchivesFrom = DateOnly.FromDateTime((DateTime)((sender as DatePicker)!).SelectedDate!);
        _ = LoadCurrentAppointments();
    }
    
    public void SelectedDateArchivesTo(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateArchivesTo = DateOnly.FromDateTime((DateTime)((sender as DatePicker)!).SelectedDate!);
    }
}