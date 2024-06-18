using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using FinalLab.Model;
using FinalLab.View.Cards;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class ChoosingDoctorViewModel : BindingHelper
{
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

    private int _idDoctor;
    private int _idAppointment;
    public ChoosingDoctorViewModel(int idSpeciality, int idDoctor, int idAppointment)
    {
        _idDoctor = idDoctor;
        _idAppointment = idAppointment;
        LoadDoctorsCards();
    }

    public void LoadDoctorsCards()
    {
        var doctors = ApiHelper.Get<List<Doctor>>("Doctors");
        foreach (var doctor in doctors)
        {
            var card = new ChoosingDoctorView($"{doctor.Surname} {doctor.FirstName} {doctor.Patronymic}", "", doctor.WorkAddress, doctor.IdDoctor);
            card.SelectionDoctor += (sender, args) => SelectionDoctor(sender, args);
            Doctors.Add(card);
        }
    }

    private void SelectionDoctor(object sender, EventArgs args)
    {
        var card = sender as ChoosingDoctorView;
        _idDoctor = card!.IdDoctor;
        FioDoctor = card!.FIO;
        
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        
        int daysOfWeek = 7 - (int)today.DayOfWeek;
        
        for (int i = 1; i <= daysOfWeek; i++)
        {
            var button = new ToggleButton();
            button.Content = today.AddDays(i).ToString("dd MMMM, ddd", new CultureInfo("ru-RU"));
            button.Style = (Style)Application.Current.Resources["ClearToggleButton"];
            CurrentWeek.Add(button);
        }
        
        for (int i = 1; i <= 7; i++)
        {
            var button = new ToggleButton();
            button.Style = (Style)Application.Current.Resources["ClearToggleButton"];
            button.Content = today.AddDays(i).ToString("dd MMMM, ddd", new CultureInfo("ru-RU"));
            NextWeek.Add(button);
        }
    }
}