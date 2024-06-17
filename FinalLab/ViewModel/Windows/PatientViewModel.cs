using FinalLab.Model;
using FinalLab.Properties;
using Newtonsoft.Json;
using SecondLibPractice;

namespace FinalLab.ViewModel.Windows;

public class PatientViewModel : BindingHelper
{
    private List<Patient> _patients = null!;

    public event EventHandler SwitchUsers;

    public List<Patient> Patients
    {
        get => _patients;
        set => SetField(ref _patients, value);
    }
    public PatientViewModel()
    {
        Patients = JsonConvert.DeserializeObject<List<Patient>>(Settings.Default.CurrentUsers)!;
    }

    public void SelectionPatient()
    {
        SwitchUsers(this, EventArgs.Empty);
    }
}