﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using FinalLab.Model;
using FinalLab.View;
using FinalLab.View.Cards;
using FinalLab.View.Pages;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class MakeAppointmentViewModel : BindingHelper
{
    #region Variables

    private ObservableCollection<SpecialtyDoctor> _specialtyDoctorCards = new();

    public ObservableCollection<SpecialtyDoctor> SpecialtyDoctorCards
    {
        get => _specialtyDoctorCards;
        set => SetField(ref _specialtyDoctorCards, value);
    }

    private ObservableCollection<Data> _currentRecords;

    public ObservableCollection<Data> CurrentRecords
    {
        get => _currentRecords;
        set => SetField(ref _currentRecords, value);
    }

    private ObservableCollection<Data> _archivedRecords;

    public ObservableCollection<Data> ArchivedRecords
    {
        get => _archivedRecords;
        set => SetField(ref _archivedRecords, value);
    }

    private readonly long _oms;
    private DateOnly _selectionDateCurrentFrom = DateOnly.FromDateTime(DateTime.Now);
    private DateOnly _selectionDateCurrentTo = DateOnly.MaxValue;
    private DateOnly _selectionDateArchivesFrom = DateOnly.FromDateTime(DateTime.Now);
    private DateOnly _selectionDateArchivesTo = DateOnly.MaxValue;

    #endregion

    #region Methods

    public MakeAppointmentViewModel()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        _oms = (window.PatientsComboBox.SelectedItem as Patient).Oms;
        window.WindowTextBlock.Text = "Запись";
        _ = LoadSpecialities();
        _ = LoadCurrentAppointments();
        _ = LoadArchivesAppointments();
    }

    private async Task LoadSpecialities()
    {
        var directions = ApiHelper.Get<List<Direction>>("Directions");
        var directionsSorted = directions!.Where(item => item.Oms == _oms);
        var specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        foreach (var item in directionsSorted!)
        {
            var specialtyDoctor =
                new SpecialtyDoctor(specialities![(int)(item.SpecialityId - 1)!].NumberImage.ToString(),
                    specialities[(int)(item.SpecialityId - 1)!].NameSpecialities, (int)item.SpecialityId!);
            specialtyDoctor.Click += (sender, args) => RecordingDirection(sender, args);
            SpecialtyDoctorCards.Add(specialtyDoctor);
        }
    }

    private async Task LoadCurrentAppointments()
    {
        CurrentRecords = new ObservableCollection<Data>();
        var appointments = ApiHelper.Get<List<Appointment>>("Appointments")!.Where(item =>
                (int)item.StatusId! != 4 && item.AppointmentDate <= _selectionDateCurrentTo &&
                item.AppointmentDate >= _selectionDateCurrentFrom && item.Oms == _oms)
            .OrderBy(item => item.AppointmentDate)
            .ToList();
        if (appointments.Count == 0)
            return;
        ObservableCollection<Appointments> monthAppointments = new();
        var month = appointments[0].AppointmentDate.Month;
        foreach (var appointment in appointments!)
        {
            var doctor = ApiHelper.Get<Doctor>("Doctors", (long)appointment.DoctorId!);
            var speciality = ApiHelper.Get<Speciality>("Specialities", (int)doctor!.SpecialityId!)!.NameSpecialities;
            if (month == appointment.AppointmentDate.Month)
            {
                var elem = new Appointments(speciality, $"{doctor.Surname} {doctor.FirstName} {doctor.Patronymic}",
                    appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, (int)doctor.IdDoctor,
                    (int)appointment.IdAppointment!);
                monthAppointments.Add(elem);
                elem.Delete += (sender, args) => Delete(sender, args);
                elem.Move += (sender, args) => Move(sender, args);
            }
            else if (month != appointment.AppointmentDate.Month)
            {
                month = appointment.AppointmentDate.Month;
                CurrentRecords.Add(new Data(
                    appointments[appointments.IndexOf(appointment) - 1].AppointmentDate.ToString("MMMM yyyy"),
                    new ObservableCollection<Appointments>(monthAppointments)));
                monthAppointments.Clear();
                var elem = new Appointments(speciality, $"{doctor.Surname} {doctor.FirstName} {doctor.Patronymic}",
                    appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, (int)doctor.IdDoctor,
                    (int)appointment.IdAppointment!);
                monthAppointments.Add(elem);
                elem.Delete += (sender, args) => Delete(sender, args);
                elem.Move += (sender, args) => Move(sender, args);
            }

            if (appointments.Count - 1 == appointments.IndexOf(appointment))
                CurrentRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"),
                    new ObservableCollection<Appointments>(monthAppointments)));
        }
    }

    private async Task LoadArchivesAppointments()
    {
        ArchivedRecords = new ObservableCollection<Data>();
        var appointments = ApiHelper.Get<List<Appointment>>("Appointments")!.Where(item =>
                (int)item.StatusId! == 4 && item.AppointmentDate <= _selectionDateArchivesTo &&
                item.AppointmentDate >= _selectionDateArchivesFrom && item.Oms == _oms)
            .OrderBy(item => item.AppointmentDate).ToList();
        if (appointments.Count == 0)
            return;
        ObservableCollection<RecordsArchive> recordsArchives = new();
        var month = appointments[0].AppointmentDate.Month;
        foreach (var appointment in appointments!)
        {
            var doctor = ApiHelper.Get<Doctor>("Doctors", (long)appointment.DoctorId!);
            var speciality = ApiHelper.Get<Speciality>("Specialities", (long)doctor!.SpecialityId!)!.NameSpecialities;
            if (month == appointment.AppointmentDate.Month)
            {
                var elem = new RecordsArchive(speciality, $"{doctor.Surname} {doctor.FirstName} {doctor.Patronymic}",
                    appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, (int)doctor.IdDoctor,
                    (int)appointment.IdAppointment!);
                recordsArchives.Add(elem);
                elem.Delete += (sender, args) => Delete(sender, args);
                elem.Repeat += (sender, args) => Repeat(sender, args);
            }
            else if (month != appointment.AppointmentDate.Month)
            {
                month = appointment.AppointmentDate.Month;
                ArchivedRecords.Add(new Data(
                    appointments[appointments.IndexOf(appointment) - 1].AppointmentDate.ToString("MMMM yyyy"),
                    new ObservableCollection<RecordsArchive>(recordsArchives)));
                recordsArchives.Clear();
                var elem = new RecordsArchive(speciality,
                    $"{doctor.Surname} {doctor.FirstName} {doctor.Patronymic}",
                    appointment.AppointmentDate.ToString("dd MMMM"), doctor.WorkAddress, (int)doctor.IdDoctor,
                    (int)appointment.IdAppointment!);
                recordsArchives.Add(elem);
                elem.Delete += (sender, args) => Delete(sender, args);
                elem.Repeat += (sender, args) => Repeat(sender, args);
            }

            if (appointments.Count - 1 == appointments.IndexOf(appointment))
                ArchivedRecords.Add(new Data(appointment.AppointmentDate.ToString("MMMM yyyy"),
                    new ObservableCollection<RecordsArchive>(recordsArchives)));
        }
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
        var card = sender as RecordsArchive;
        var doctor = ApiHelper.Get<Doctor>("Doctors", card!.IdDoctor);
        Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault()!.PageFrame.Content =
            new ChoosingDoctorPage((int)doctor!.SpecialityId!, card!.IdDoctor);
    }

    private void Move(object sender, EventArgs args)
    {
        var card = sender as Appointments;
        var doctor = ApiHelper.Get<Doctor>("Doctors", card!.IdDoctor);
        Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault()!.PageFrame.Content =
            new ChoosingDoctorPage((int)doctor!.SpecialityId!, card!.IdDoctor, card.IdAppointment);
    }

    private void RecordingDirection(object sender, EventArgs args)
    {
        var card = sender as SpecialtyDoctor;
        Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault()!.PageFrame.Content =
            new ChoosingDoctorPage(card!.IdSpeciality);
    }

    public async void SelectedDateCurrentFrom(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateCurrentFrom = DateOnly.FromDateTime((DateTime)(sender as DatePicker)!.SelectedDate!);
        await LoadCurrentAppointments();
    }

    public async void SelectedDateCurrentTo(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateCurrentTo = DateOnly.FromDateTime((DateTime)(sender as DatePicker)!.SelectedDate!);
        await LoadCurrentAppointments();
    }

    public async void SelectedDateArchivesFrom(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateArchivesFrom = DateOnly.FromDateTime((DateTime)(sender as DatePicker)!.SelectedDate!);
        await LoadArchivesAppointments();
    }

    public async void SelectedDateArchivesTo(object? sender, SelectionChangedEventArgs e)
    {
        _selectionDateArchivesTo = DateOnly.FromDateTime((DateTime)(sender as DatePicker)!.SelectedDate!);
        await LoadArchivesAppointments();
    }

    #endregion
}