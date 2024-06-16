using System.IO;
using System.Net.Http.Json;
using System.Windows.Controls;
using FinalLab.Model;
using Newtonsoft.Json;
using SecondLibPractice;

namespace FinalLab.ViewModel;

public class AdministratorViewModel : BindingHelper
{
    #region MyRegion

    public event EventHandler SwitchForm;

    private List<string> _items = new List<string>(){ "Пользователь", "Доктор", "Администратор"};

    private List<Speciality>? _specialities;

    public List<Speciality>? Specialities
    {
        get => _specialities;
        set => SetField(ref _specialities, value);
    }
    public List<string> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }

    private string _selectedRole;

    private int _idSpeciality;

    public int IdSpeciality
    {
        get => _idSpeciality;
        set => SetField(ref _idSpeciality, value);
    }

    public string SelectedRole
    {
        get => _selectedRole;
        set => SetField(ref _selectedRole, value);
    }

    private Admin _adminItem;

    public Admin AdminItem
    {
        get => _adminItem;
        set => SetField(ref _adminItem, value);
    }
    
    private Patient _patientItem;

    public Patient PatientItem
    {
        get => _patientItem;
        set => SetField(ref _patientItem, value);
    }
    
    private Doctor _doctorItem;

    public Doctor DoctorItem
    {
        get => _doctorItem;
        set => SetField(ref _doctorItem, value);
    }

    private List<object>? _obj;

    public List<object>? Obj
    {
        get => _obj;
        set => SetField(ref _obj, value);
    }
    #endregion
    public AdministratorViewModel()
    {
        SelectedRole = "Пользователь";
        Obj = ApiHelper.Get<List<Object>>("Patients");
        Specialities = ApiHelper.Get<List<Speciality>>("Specialities");
    }

    public void SelectionRole()
    {
        if (SelectedRole == "Пользователь")
            Obj = ApiHelper.Get<List<Object>>("Patients");
        else if (SelectedRole == "Доктор")
            Obj = ApiHelper.Get<List<Object>>("Doctors");
        else
            Obj = ApiHelper.Get<List<Object>>("Admins");
        
        SwitchForm(this, EventArgs.Empty);
    }

    public void SelectionDataGrid(object sender, SelectionChangedEventArgs e)
    {
        if (((sender as DataGrid)!).SelectedItem == null)
            return;
        if (SelectedRole == "Пользователь")
            PatientItem = JsonConvert.DeserializeObject<Patient>((sender as DataGrid).SelectedItem.ToString());
        else if (SelectedRole == "Доктор")
        {
            DoctorItem = JsonConvert.DeserializeObject<Doctor>((sender as DataGrid).SelectedItem.ToString());
            IdSpeciality = (int)DoctorItem.SpecialityId - 1;
        }
        else
            AdminItem = JsonConvert.DeserializeObject<Admin>((sender as DataGrid).SelectedItem.ToString());
    }

    public void Add()
    {
        bool result = false;
        if (SelectedRole == "Пользователь")
        {
            string json = JsonConvert.SerializeObject(PatientItem);
            result = ApiHelper.Post(json, "Patients");
        }
        else if (SelectedRole == "Доктор")
        {
            string json = JsonConvert.SerializeObject(DoctorItem);
            result = ApiHelper.Post(json, "Doctors");
        }
        else
        {
            string json = JsonConvert.SerializeObject(AdminItem);
            result = ApiHelper.Post(json, "Admins");
        }

        if (result)
            SelectionRole();
    }

    public void Update()
    {
        bool result = false;
        if (SelectedRole == "Пользователь")
        {
            string json = JsonConvert.SerializeObject(PatientItem);
            result = ApiHelper.Put(json, "Patients", PatientItem.Oms);
        }
        else if (SelectedRole == "Доктор")
        {
            string json = JsonConvert.SerializeObject(DoctorItem);
            result = ApiHelper.Put(json, "Doctors", (long)DoctorItem.IdDoctor!);
        }
        else
        {
            string json = JsonConvert.SerializeObject(AdminItem);
            result = ApiHelper.Put(json, "Admins", (long)AdminItem.IdAdmin!);
        }

        SelectionRole();
    }

    public void Delete()
    {
        bool result = false;
        if (SelectedRole == "Пользователь")
        {
            result = ApiHelper.Delete( "Patients", PatientItem.Oms);
        }
        else if (SelectedRole == "Доктор")
        {
            result = ApiHelper.Delete("Doctors", (long)DoctorItem.IdDoctor!);
        }
        else
        {
            result = ApiHelper.Delete( "Admins", (long)AdminItem.IdAdmin!);
        }

        if (result)
            SelectionRole();
    }
}