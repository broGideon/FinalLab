﻿using System.Windows;

namespace FinalLab.View.Cards;
public partial class Appointments_Control
{
    public string NameResearch { get; set; }
    
    public string FIO { get; set; }
    
    public string Date { get; set; }
    
    public string Address { get; set; }

    public int IdAppointment;

    public event EventHandler Click;
    public Appointments_Control(string nameResearch, string fio, string date, string address, int idAppointment)
    {
        InitializeComponent();
        NameResearch = nameResearch;
        FIO = fio;
        Date = date;
        Address = address;
        IdAppointment = idAppointment;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Click(this, EventArgs.Empty);
    }
}