﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using FinalLab.Model;
using FinalLab.View;
using FinalLab.View.Cards;
using Newtonsoft.Json;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class ChoosingDoctorViewModel : BindingHelper
{
    #region Variables

    private ObservableCollection<ToggleButton> _currentWeek = new();

    public ObservableCollection<ToggleButton> CurrentWeek
    {
        get => _currentWeek;
        set => SetField(ref _currentWeek, value);
    }

    private ObservableCollection<ToggleButton> _nextWeek = new();

    public ObservableCollection<ToggleButton> NextWeek
    {
        get => _nextWeek;
        set => SetField(ref _nextWeek, value);
    }

    private string _fioDoctor;

    public string FioDoctor
    {
        get => _fioDoctor;
        set => SetField(ref _fioDoctor, value);
    }

    private ObservableCollection<ChoosingDoctorView> _doctors = new();

    public ObservableCollection<ChoosingDoctorView> Doctors
    {
        get => _doctors;
        set => SetField(ref _doctors, value);
    }

    private ObservableCollection<ToggleButton> _morning = new();

    public ObservableCollection<ToggleButton> Morning
    {
        get => _morning;
        set => SetField(ref _morning, value);
    }

    private ObservableCollection<ToggleButton> _day = new();

    public ObservableCollection<ToggleButton> Day
    {
        get => _day;
        set => SetField(ref _day, value);
    }

    private ObservableCollection<ToggleButton> _evening = new();

    public ObservableCollection<ToggleButton> Evening
    {
        get => _evening;
        set => SetField(ref _evening, value);
    }

    private int _idDoctor;

    private readonly int _idAppointment;

    private readonly long _oms;

    private ToggleButton _selectedDay = new();

    private ToggleButton _selectedTime = new();

    #endregion

    #region Methods

    public ChoosingDoctorViewModel(int idSpeciality, int idDoctor, int idAppointment)
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        _oms = (window.PatientsComboBox.SelectedItem as Patient).Oms;
        window.WindowTextBlock.Text =
            $"Выбор специалиста - {ApiHelper.Get<Speciality>("Specialities", idSpeciality)!.NameSpecialities}";
        _idDoctor = idDoctor;
        _idAppointment = idAppointment;
        LoadDoctorsCards(idSpeciality);
        if (idDoctor != -1)
        {
            LoadDateToggleButton();
            var doctor = ApiHelper.Get<Doctor>("Doctors", idDoctor);
            FioDoctor = $"{doctor.Surname} {doctor.FirstName} {doctor.Patronymic}";
        }
    }

    public void LoadDoctorsCards(int idSpeciality)
    {
        var doctors = ApiHelper.Get<List<Doctor>>("Doctors")!.Where(item => item.SpecialityId == idSpeciality).ToList();
        foreach (var doctor in doctors)
        {
            var card = new ChoosingDoctorView($"{doctor.Surname} {doctor.FirstName} {doctor.Patronymic}", "Сегодня",
                doctor.WorkAddress, (int)doctor.IdDoctor!);
            card.SelectionDoctor += (sender, args) => SelectionDoctor(sender, args);
            Doctors.Add(card);
        }
    }

    private void SelectionDoctor(object sender, EventArgs args)
    {
        var card = sender as ChoosingDoctorView;
        _idDoctor = card!.IdDoctor;
        FioDoctor = card!.FIO;
        CurrentWeek = new ObservableCollection<ToggleButton>();
        NextWeek = new ObservableCollection<ToggleButton>();
        Morning = new ObservableCollection<ToggleButton>();
        Day = new ObservableCollection<ToggleButton>();
        Evening = new ObservableCollection<ToggleButton>();
        LoadDateToggleButton();
    }

    private void LoadDateToggleButton()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var daysOfWeek = 7 - (int)today.DayOfWeek;

        for (var i = 1; i <= daysOfWeek; i++)
        {
            var button = new ToggleButton();
            button.Style = (Style)Application.Current.Resources["ClearToggleButton"];
            button.Content = today.AddDays(i).ToString("dd MMMM, ddd", new CultureInfo("ru-RU"));
            button.Click += (sender, args) => SelectionDay(sender, args);
            CurrentWeek.Add(button);
        }

        for (var i = 1 + daysOfWeek; i <= 7 + daysOfWeek; i++)
        {
            var button = new ToggleButton();
            button.Style = (Style)Application.Current.Resources["ClearToggleButton"];
            button.Content = today.AddDays(i).ToString("dd MMMM, ddd", new CultureInfo("ru-RU"));
            button.Click += (sender, args) => SelectionDay(sender, args);
            NextWeek.Add(button);
        }
    }

    private void LoadTimeToggleButton()
    {
        var currentDate = DateOnly.FromDateTime(DateTime.ParseExact(_selectedDay.Content.ToString()!, "dd MMMM, ddd",
            new CultureInfo("ru-RU")));
        var appointments = ApiHelper.Get<List<Appointment>>("Appointments")!
            .Where(item => item.AppointmentDate == currentDate && item.DoctorId == _idDoctor)
            .OrderBy(item => item.AppointmentTime).ToList();
        var indexAppointment = 0;
        var defTime = new TimeOnly(7, 50);
        for (var i = 0; i < 25; i++)
        {
            defTime = defTime.AddMinutes(10);
            if (appointments.Count != 0)
                if (appointments.Count != indexAppointment && appointments[indexAppointment].AppointmentTime == defTime)
                {
                    indexAppointment++;
                    continue;
                }

            var button = new ToggleButton();
            button.Style = (Style)Application.Current.Resources["ClearToggleButton"];
            button.Content = defTime.ToString("HH:mm", new CultureInfo("ru-RU"));
            button.Click += (sender, args) => SelectionTime(sender, args);
            Morning.Add(button);
        }

        for (var i = 0; i < 30; i++)
        {
            defTime = defTime.AddMinutes(10);
            if (appointments.Count != 0)
                if (appointments.Count != indexAppointment && appointments[indexAppointment].AppointmentTime == defTime)
                {
                    indexAppointment++;
                    continue;
                }

            var button = new ToggleButton();
            button.Style = (Style)Application.Current.Resources["ClearToggleButton"];
            button.Content = defTime.ToString("HH:mm", new CultureInfo("ru-RU"));
            button.Click += (sender, args) => SelectionTime(sender, args);
            Day.Add(button);
        }

        for (var i = 0; i < 18; i++)
        {
            defTime = defTime.AddMinutes(10);
            if (appointments.Count != 0)
                if (appointments.Count != indexAppointment && appointments[indexAppointment].AppointmentTime == defTime)
                {
                    indexAppointment++;
                    continue;
                }

            var button = new ToggleButton();
            button.Style = (Style)Application.Current.Resources["ClearToggleButton"];
            button.Content = defTime.ToString("HH:mm", new CultureInfo("ru-RU"));
            button.Click += (sender, args) => SelectionTime(sender, args);
            Evening.Add(button);
        }
    }

    private void SelectionDay(object sender, RoutedEventArgs e)
    {
        var item = sender as ToggleButton;
        Morning = new ObservableCollection<ToggleButton>();
        Day = new ObservableCollection<ToggleButton>();
        Evening = new ObservableCollection<ToggleButton>();
        if (item.IsChecked == true)
        {
            _selectedDay.IsChecked = false;
            _selectedDay = item;
            LoadTimeToggleButton();
        }
        else
        {
            _selectedDay = new ToggleButton();
        }
    }

    private void SelectionTime(object sender, RoutedEventArgs e)
    {
        var item = sender as ToggleButton;
        if (item.IsChecked == true)
        {
            _selectedTime.IsChecked = false;
            _selectedTime = item;
        }
        else
        {
            _selectedTime = new ToggleButton();
        }
    }

    public void MakeAppointment()
    {
        if (_selectedDay.Content == null ||
            _selectedTime.Content == null || _idDoctor == -1)
            return;
        var currentDate = DateOnly.FromDateTime(DateTime.ParseExact(_selectedDay.Content.ToString()!, "dd MMMM, ddd",
            new CultureInfo("ru-RU")));
        var currentTime =
            TimeOnly.FromDateTime(DateTime.ParseExact(_selectedTime.Content.ToString()!+":00", "HH:mm:ss",
                new CultureInfo("ru-RU")));
        var result = false;
        
        if (_idAppointment != -1)
        {
            var json = JsonConvert.SerializeObject(new Appointment(_idAppointment, currentDate, currentTime, _oms,
                _idDoctor, 1));
            result = ApiHelper.Put(json, "Appointments", _idAppointment);
        }
        else
        {
            var json = JsonConvert.SerializeObject(new Appointment(currentDate, currentTime, _oms, _idDoctor, 1));
            result = ApiHelper.Post(json, "Appointments");
        }

        if (result)
        {
            _selectedDay = new();
            _selectedTime = new();
            CurrentWeek = new ObservableCollection<ToggleButton>();
            NextWeek = new ObservableCollection<ToggleButton>();
            Morning = new ObservableCollection<ToggleButton>();
            Day = new ObservableCollection<ToggleButton>();
            Evening = new ObservableCollection<ToggleButton>();
            LoadDateToggleButton();
        }
    }

    #endregion
}