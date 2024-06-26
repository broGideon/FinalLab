﻿using System.Windows;
using FinalLab.Model;
using FinalLab.Properties;
using Newtonsoft.Json;
using SecondLibPractice;
using Wpf.Ui.Controls;

namespace FinalLab.ViewModel;

public class MainViewModel : BindingHelper
{
    #region Variables

    public bool IsAddAccount { get; set; }
    public event EventHandler OpenClientWindow;
    public event EventHandler OpenDoctorWindow;
    public event EventHandler OpenAdminWindow;
    public event EventHandler SwitchPage;

    private string _password;

    private string _oms;

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

    #region Methods

    public MainViewModel(bool isAddAccount)
    {
        IsAddAccount = !isAddAccount;
    }

    public void AuthClient()
    {
        long oms;
        if (!long.TryParse(Oms, out oms) || Oms == "0")
            return;

        var client = ApiHelper.Get<Patient>("Patients", oms);
        if (client == null) return;

        if (string.IsNullOrEmpty(Settings.Default.CurrentUsers))
        {
            Settings.Default.CurrentUsers = JsonConvert.SerializeObject(new List<Patient> { client });
        }
        else
        {
            var users = JsonConvert.DeserializeObject<List<Patient>>(Settings.Default.CurrentUsers);
            if (!users.Exists(item => item.Oms == oms))
                users!.Add(client);
            Settings.Default.CurrentUsers = JsonConvert.SerializeObject(users);
        }

        Settings.Default.Save();
        OpenClientWindow(this, EventArgs.Empty);
    }

    public void AuthPersonal()
    {
        long login;
        if (!long.TryParse(Login, out login) || Login == "0")
            return;

        var doctor = ApiHelper.Get<Doctor>("Doctors", login);
        if (doctor != null && doctor.EnterPassword == _password)
        {
            Settings.Default.CurrentDoctor = (int)doctor.IdDoctor!;
            Settings.Default.Save();
            OpenDoctorWindow(this, EventArgs.Empty);
            return;
        }

        var admin = ApiHelper.Get<Admin>("Admins", login);
        if (admin != null && admin.EnterPassword == _password)
        {
            Settings.Default.CurrentAdmin = (int)admin.IdAdmin!;
            Settings.Default.Save();
            OpenAdminWindow(this, EventArgs.Empty);
        }
    }

    public void SwitchPageMethod()
    {
        SwitchPage(this, EventArgs.Empty);
    }

    #endregion
}