﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FinalLab.View.Pages;
using FinalLab.ViewModel;
using FinalLab.ViewModel.Windows;

namespace FinalLab.View;

public partial class PatientWindow : Window
{
    private PatientViewModel _viewModel;
    
    public PatientWindow()
    {
        InitializeComponent();
        _viewModel = new PatientViewModel();
        _viewModel.SwitchUsers += (sender, args) => OpenHomePatient(sender, args);
        DataContext = _viewModel;
        //PageFrame.Content = new HomePatientPage();
    }

    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        PageFrame.Content = new ProfilePage(_viewModel);
    }

    private void OpenHomePatient(object sender, EventArgs e)
    {
        PageFrame.Content = new HomePatientPage();
    }

    private void OpenAppointments(object sender, MouseButtonEventArgs e)
    {
        PageFrame.Content = new AppointmentPage();
    }

    private void OpenAnalyzes(object sender, MouseButtonEventArgs e)
    {
        PageFrame.Content = new AnalyzePage();
    }

    private void OpenResearches(object sender, MouseButtonEventArgs e)
    {
        PageFrame.Content = new ResearchePage();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MoveWindow(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }

    private void UnwrapButton_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Normal)
            WindowState = WindowState.Maximized;
        else
            WindowState = WindowState.Normal;
    }

    private void RollUpButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}