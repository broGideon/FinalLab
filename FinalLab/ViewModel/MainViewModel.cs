using System.Windows;
using FinalLab.Model;
using SecondLibPractice;
using Wpf.Ui.Controls;

namespace FinalLab.ViewModel;

public class MainViewModel : BindingHelper
{
    #region ParamApp

    public event EventHandler OpenClientWindow; 
    public event EventHandler OpenDoctorWindow; 
    public event EventHandler OpenAdminWindow; 
    
    private string _password;

    private string _oms;

    private string _error;

    public string Error
    {
        get => _error;
        set => SetField(ref _error, value);
    }

    public string Oms
    {
        get => _oms;
        set => SetField(ref _oms, value);
    }

    private string _login;

    public string Login
    {
        get => _login;
        set => SetField(ref _login, value);
    }
    
    public void PasswordChanged(object sender, RoutedEventArgs e)
    {
        _password = (sender as PasswordBox).Password;
    }
    #endregion

    public void AuthClient()
    {
        long oms;
        if (!long.TryParse(Oms, out oms))
        {
            Error = "Неверный формат ОМС";
            return;
        }
        
        Patient? client = ApiHelper.Get<Patient>("Patients", oms);
        if (client == null)
        {
            Error = "Такого аккаунта нет";
            return;
        }
        else
            OpenClientWindow(this, EventArgs.Empty);
    }
    
    public void AuthPersonal()
    {
        long login;
        if (!long.TryParse(Login, out login))
        {
            Error = "Номер это число";
            return;
        }
        
        Doctor? doctor = ApiHelper.Get<Doctor>("Doctors", login);
        if (doctor != null && doctor.EnterPassword == _password)
        {
            OpenDoctorWindow(this, EventArgs.Empty);
            return;
        }

        Admin? admin = ApiHelper.Get<Admin>("Admins", login);
        if (admin != null && admin.EnterPassword == _password)
        {
            OpenAdminWindow(this, EventArgs.Empty);
            return;
        }

        Error = "Такаго аккаунта нет";
    }
}