using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using FinalLab.Model;
using FinalLab.Properties;
using Newtonsoft.Json;
using SecondLibPractice;

namespace FinalLab.ViewModel.Windows;

public class PatientViewModel : BindingHelper
{
    #region Variables
    
    public List<string> Themes { get; set; } = new List<string> { "Светлая", "Темная"};
    
    public event EventHandler SwitchUsers;

    private ObservableCollection<Patient> _patients = null!;

    public ObservableCollection<Patient> Patients
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

    public event EventHandler Close;
    
    #endregion

    #region Methods
    
    public PatientViewModel()
    {
        Patients = JsonConvert.DeserializeObject<ObservableCollection<Patient>>(Settings.Default.CurrentUsers)!;
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

    public void CancelAccount()
    {
        if (Patients.Count == 1)
        {
            Settings.Default.CurrentUsers = String.Empty;   
            Settings.Default.Save();
            Close(this, EventArgs.Empty);
            return;
        }

        var patient = CurrentPatient;
        if (Patients.IndexOf(patient) == 0)
            CurrentPatient = Patients[1];
        else
            CurrentPatient = Patients[0];
        Patients.Remove(patient);
        Settings.Default.CurrentUsers = JsonConvert.SerializeObject(Patients);
        Settings.Default.Save();
    }

    public void Update()
    {
        if (!string.IsNullOrEmpty(CurrentPatient.Email) && !string.IsNullOrEmpty(CurrentPatient.Phone) &&
            !string.IsNullOrEmpty(CurrentPatient.AddressPatient) && !string.IsNullOrEmpty(CurrentPatient.LivingAddress))
            ApiHelper.Put(JsonConvert.SerializeObject(CurrentPatient), "Patients", CurrentPatient.Oms);
        else
            MessageBox.Show("Все поля должны быть заполнены");
    }

    public void Copy()
    {
        CurrentPatient.LivingAddress = CurrentPatient.AddressPatient;
    }
    
    #endregion
}
