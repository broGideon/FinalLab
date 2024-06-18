using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using FinalLab.Model;
using FinalLab.View;
using FinalLab.View.Cards;
using Microsoft.Win32;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class ResearcheViewModel : BindingHelper
{
    #region MyRegion

    private bool _downloadEnable = false;

    public bool DownloadEnable
    {
        get => _downloadEnable;
        set => SetField(ref _downloadEnable, value);
    }

    private string _researchName;
    
    public string ResearchName
    {
        get => _researchName;
        set => SetField(ref _researchName, value);
    }

    private string _address;

    public string Address
    {
        get => _address;
        set => SetField(ref _address, value);
    }

    private string _doctor;

    public string Doctor
    {
        get => _doctor;
        set => SetField(ref _doctor, value);
    }

    private string _date;

    public string Date
    {
        get => _date;
        set => SetField(ref _date, value);
    }

    public FlowDocument RTB { get; set; }

    private ObservableCollection<Appointments_Control> _elements = new();

    public ObservableCollection<Appointments_Control> Elements
    {
        get => _elements;
        set => SetField(ref _elements, value);
    }

    private long _oms;

    private int _id;

    public ResearcheViewModel()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        _oms = (window.PatientsComboBox.SelectedItem as Patient).Oms;
        window.WindowTextBlock.Text = "Исследования";
        RTB = new();
        LoadCards();
    }
    #endregion

    public void DownloadFile()
    {
        var dialog = new SaveFileDialog
        {
            Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg;*.jpeg)|*.jpg;*.jpeg|Bitmap Image (*.bmp)|*.bmp",
            Title = "Save an Image File"
        };
        var result = dialog.ShowDialog();

        if (result == true)
        {
            MemoryStream ms = new MemoryStream(ApiHelper.Get<ResearchDocument>("ResearchDocuments", _id)!.Attachment!);
            Image image = Image.FromStream(ms);
            try
            {
                image.Save(dialog.FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            ms.Dispose();
        }
    }

    private void LoadCards()
    {
        var appointments = ApiHelper.Get<List<Appointment>>("Appointments")!.Where(item => item.Oms == _oms).OrderBy(item => item.AppointmentDate).ToList();
        foreach (var appointment in appointments)
        {
            var researchDocument =
                ApiHelper.Get<ResearchDocument>("ResearchDocuments", (long)appointment.IdAppointment!);
            if (researchDocument != null)
            {
                var doctor = ApiHelper.Get<Doctor>("Doctors", (long)appointment.DoctorId!);
                var card = new Appointments_Control(researchDocument.DocumentName, $"{doctor!.Surname} {doctor.FirstName.Substring(0, 1)}. {doctor.Patronymic.Substring(0, 1)}.", appointment.AppointmentDate.ToString("dd MMMM yyyy"), doctor.WorkAddress, (int)appointment.IdAppointment);
                card.Click += (sender, args) => LoadInfo(sender, args);
                Elements.Add(card);
            }
        }
    }

    private void LoadInfo(object sender, EventArgs args)
    {
        var card = sender as Appointments_Control;
        _id = card.IdAppointment;
        ResearchName = card.NameResearch;
        Doctor = card.FIO;
        Address = card.Address;
        Date = card.Date;
        var document = ApiHelper.Get<ResearchDocument>("ResearchDocuments", card.IdAppointment);
        DownloadEnable = document.Attachment != null ? true : false;
        File.WriteAllText("buffer.rtf", document.Rtf);
        var range = new TextRange(RTB.ContentStart, RTB.ContentEnd);
        var fs = new FileStream("buffer.rtf", FileMode.Open);
        range.Load(fs, DataFormats.Rtf);
        fs.Close();
        File.Delete("buffer.rtf");
    }
}