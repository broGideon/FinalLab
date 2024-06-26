﻿using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using FinalLab.Model;
using FinalLab.View;
using FinalLab.View.Cards;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class AnalyseViewModel : BindingHelper
{
    #region Variables

    private string _analyseName;

    public string AnalyseName
    {
        get => _analyseName;
        set => SetField(ref _analyseName, value);
    }

    private string _address;

    public string Address
    {
        get => _address;
        set => SetField(ref _address, value);
    }

    private string _date;

    public string Date
    {
        get => _date;
        set => SetField(ref _date, value);
    }

    public FlowDocument RTB { get; set; }

    private ObservableCollection<AnalyseControl> _elements = new();

    public ObservableCollection<AnalyseControl> Elements
    {
        get => _elements;
        set => SetField(ref _elements, value);
    }

    private readonly long _oms;

    private int _id;

    #endregion

    #region Methods

    public AnalyseViewModel()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        _oms = (window.PatientsComboBox.SelectedItem as Patient).Oms;
        window.WindowTextBlock.Text = "Анализы";
        RTB = new FlowDocument();
        LoadCards();
    }

    private void LoadCards()
    {
        var appointments = ApiHelper.Get<List<Appointment>>("Appointments")!.Where(item => item.Oms == _oms)
            .OrderBy(item => item.AppointmentDate);
        foreach (var appointment in appointments)
        {
            var analysDocument =
                ApiHelper.Get<ResearchDocument>("AnalysDocuments", (long)appointment.IdAppointment!);
            if (analysDocument != null)
            {
                var doctor = ApiHelper.Get<Doctor>("Doctors", (long)appointment.DoctorId!);
                var card = new AnalyseControl(analysDocument.DocumentName,
                    appointment.AppointmentDate.ToString("dd MMMM yyyy"), (int)analysDocument.IdAppointment!,
                    doctor.WorkAddress);
                card.Click += (sender, args) => LoadInfo(sender, args);
                Elements.Add(card);
            }
        }
    }

    private void LoadInfo(object sender, EventArgs args)
    {
        var card = sender as AnalyseControl;
        _id = card.IdAppointment;
        AnalyseName = card.AnalyseName;
        Address = card.Address;
        Date = card.Date;
        var document = ApiHelper.Get<ResearchDocument>("AnalysDocuments", card.IdAppointment);
        File.WriteAllText("buffer.rtf", document.Rtf);
        var range = new TextRange(RTB.ContentStart, RTB.ContentEnd);
        var fs = new FileStream("buffer.rtf", FileMode.Open);
        range.Load(fs, DataFormats.Rtf);
        fs.Close();
        File.Delete("buffer.rtf");
    }

    #endregion
}