using System.Windows.Controls;
using FinalLab.Model;
using FinalLab.Properties;
using Newtonsoft.Json;
using SecondLibPractice;

namespace FinalLab.ViewModel.Windows;

public class PatientViewModel : BindingHelper
{
    public List<string> Themes { get; set; } = new List<string> { "Светлая", "Темная"};
    
    private List<Patient> _patients = null!;

    public event EventHandler SwitchUsers;

    public List<Patient> Patients
    {
        get => _patients;
        set => SetField(ref _patients, value);
    }

    private Patient _currentPatient;

    public Patient CurrentPatient
    {
        get => _currentPatient;
        set => SetField(ref _currentPatient, value);
    }

    private string _currentTheme;

    public string CurrentTheme
    {
        get => _currentTheme;
        set => SetField(ref _currentTheme, value);
    }
    
    public PatientViewModel()
    {
        Patients = JsonConvert.DeserializeObject<List<Patient>>(Settings.Default.CurrentUsers)!;
        CurrentPatient = Patients[0];
        if (App.Theme == "Light")
            CurrentTheme = "Светлая";
        else
            CurrentTheme = "Темная";
    }

    public void SelectionPatient(object sender, SelectionChangedEventArgs e)
    {
        CurrentPatient = (((sender as ComboBox)!).SelectedItem as Patient)!;
        SwitchUsers(this, EventArgs.Empty);
    }
    
    public void SelectionTheme(object sender, SelectionChangedEventArgs e)
    {
        if (CurrentTheme == "Светлая")
            App.Theme = "Light";
        else
            App.Theme = "Dark";
    }
}