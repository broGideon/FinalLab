using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FinalLab.Model;
using FinalLab.View.Cards;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using SecondLibPractice;
using Spire.Doc;
using Image = System.Drawing.Image;
using Spire.Doc.Documents;
using HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment;
using Paragraph = Spire.Doc.Documents.Paragraph;
using Section = Spire.Doc.Section;
using Table = Spire.Doc.Table;

namespace FinalLab.ViewModel;

public class DoctorViewModel : BindingHelper
{
    #region MyRegion

    public event EventHandler ReloadPage;
    public FlowDocument AnalyzeRTB { get; set; }

    public FlowDocument ResearchRTB { get; set; }
    
    private ObservableCollection<ClientsView> _appointments = new ObservableCollection<ClientsView>();

    public ObservableCollection<ClientsView> Appointments
    {
        get => _appointments;
        set => SetField(ref _appointments, value);
    }

    private List<Speciality>? _specialities;

    public List<Speciality>? Specialities
    {
        get => _specialities;
        set => SetField(ref _specialities, value);
    }

    private Speciality _selectSpeciality;

    public Speciality SelectSpeciality
    {
        get => _selectSpeciality;
        set => SetField(ref _selectSpeciality, value);
    }

    private ObservableCollection<ReferralView?> _directions;

    public ObservableCollection<ReferralView?> Directions
    {
        get => _directions;
        set => SetField(ref _directions, value);
    }

    private int _idDoctor;

    private string _complaints;

    public string Complaints
    {
        get => _complaints;
        set => SetField(ref _complaints, value);
    }

    private string _osmotor;

    public string Osmotor
    {
        get => _osmotor;
        set => SetField(ref _osmotor, value);
    }

    private string _primaryDiagnosis;

    public string PrimaryDiagnosis
    {
        get => _primaryDiagnosis;
        set => SetField(ref _primaryDiagnosis, value);
    }

    private string _secondaryDiagnosis;

    public string SecondaryDiagnosis
    {
        get => _secondaryDiagnosis;
        set => SetField(ref _secondaryDiagnosis, value);
    }

    private string _recommendations;

    public string Recommendations
    {
        get => _recommendations;
        set => SetField(ref _recommendations, value);
    }

    private string _patientFIO;

    public string PatientFIO
    {
        get => _patientFIO;
        set => SetField(ref _patientFIO, value);
    }

    private string _patientOMS;

    public string PatientOMS
    {
        get => _patientOMS;
        set => SetField(ref _patientOMS, value);
    }

    private string _user;

    public string User
    {
        get => _user;
        set => SetField(ref _user, $"ЕМИАС - {value}");
    }

    private bool _researchIsActive;

    public bool ResearchIsActive
    {
        get => _researchIsActive;
        set => SetField(ref _researchIsActive, value);
    }
    
    private bool _analyzeIsActive;

    public bool AnalyzeIsActive
    {
        get => _analyzeIsActive;
        set => SetField(ref _analyzeIsActive, value);
    }

    private string _nameAnalyze;

    public string NameAnalyze
    {
        get => _nameAnalyze;
        set => SetField(ref _nameAnalyze, value);
    }
    
    private string _nameResearch;

    public string NameResearch
    {
        get => _nameResearch;
        set => SetField(ref _nameResearch, value);
    }

    private Doctor _doctor;

    public DoctorViewModel(int idDoctor)
    {
        _idDoctor = idDoctor;
        Specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        AnalyzeRTB = new();
        ResearchRTB = new();
        _doctor = ApiHelper.Get<Doctor>("Doctors", idDoctor);
        User = $"{_doctor.Surname} {_doctor.FirstName} {_doctor.Patronymic}";
        Directions = new();
    }

    private DateOnly _currentDate;

    private byte[] _image;

    private ClientsView _currentClientView;
    #endregion
    public void AddDirections()
    {
        if (SelectSpeciality != null)
        {
            ReferralView referralView = new ReferralView((int)SelectSpeciality.IdSpeciality!, SelectSpeciality.NameSpecialities);
            referralView.DeleteSpeciality += (sender, args) => DeleteSpeciality(sender, args);
            Directions.Add(referralView);
        }
    }

    private void DeleteSpeciality(object sender, EventArgs args)
    {
        Directions.Remove(sender as ReferralView);
    }

    public void SelectedDate(object? sender, SelectionChangedEventArgs e)
    {
        _currentDate = DateOnly.FromDateTime((DateTime)(sender as DatePicker).SelectedDate);
        List<Appointment>? listAppointments = ApiHelper.Get<List<Appointment>>("Appointments");
        Appointments.Clear();
        foreach (var item in listAppointments)
        {
            if (item.AppointmentDate == _currentDate && item.DoctorId == _idDoctor && item.StatusId != 4)
            {
                Patient patient = ApiHelper.Get<Patient>("Patients", (long)item.Oms);
                ClientsView clientsView = new ClientsView($"{patient!.Surname} {patient.FirstName} {patient.Patronymic}", 
                    item.AppointmentTime.ToString(), (long)item.Oms, (int)item.IdAppointment!);
                clientsView.StartReception += (sender, args) => Start(sender, args);
                clientsView.CancelRecception += (sender, args) => CancelRecception(sender, args);
                Appointments.Add(clientsView);
            }
        }
    }

    public void AddDopFile()
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog { Title = "Open in Image" };
        dialog.Filters.Add(new CommonFileDialogFilter("Image Files", "*.jpg;*.jpeg;*.png;*.bmp;"));
        var result = dialog.ShowDialog();

        if (result == CommonFileDialogResult.Ok)
        {
            Image image = Image.FromFile(dialog.FileName);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, image.RawFormat);
            _image = memoryStream.ToArray();
            memoryStream.Dispose();
        }
    }

    public async void Cancel()
    {
        var currentAppointment = ApiHelper.Get<Appointment>("Appointments", _currentClientView.IdAppointment);
        _ = AddAppointmentDocument(currentAppointment.IdAppointment);
        
        if (AnalyzeIsActive)
        {
            _ = AddAnalysDocument(currentAppointment.IdAppointment);
        }

        if (ResearchIsActive)
        {
            _ = AddResearchDocuments(currentAppointment.IdAppointment);
        }
        
        currentAppointment.StatusId = 4;
        string jsonAppointment = JsonConvert.SerializeObject(currentAppointment);
        ApiHelper.Put(jsonAppointment, "Appointments", (long)currentAppointment.IdAppointment);
        
        await AddPatientDirections();
        
        _currentClientView.Cancel();
        ClearPage();
    }

    public void Start(object sender, EventArgs args)
    {
        ClearPage();
        _currentClientView = sender as ClientsView;
        PatientFIO = _currentClientView.FIO;
        PatientOMS = _currentClientView.OMS.ToString();
        var doc = new Document();
        doc.LoadFromFile("../../../Document/Document1.docx");
        doc.SaveToFile("analyzeBuffer.rtf", FileFormat.Rtf);
        var range = new TextRange(AnalyzeRTB.ContentStart, AnalyzeRTB.ContentEnd);
        var fs = new FileStream("analyzeBuffer.rtf", FileMode.Open);
        range.Load(fs, DataFormats.Rtf);
        fs.Close();
        File.Delete("analyzeBuffer.rtf");
        doc.LoadFromFile("../../../Document/Document2.docx");
        doc.SaveToFile("researchBuffer.rtf", FileFormat.Rtf);
        range = new TextRange(ResearchRTB.ContentStart, ResearchRTB.ContentEnd);
        fs = new FileStream("researchBuffer.rtf", FileMode.Open);
        range.Load(fs, DataFormats.Rtf);
        fs.Close();
        File.Delete("researchBuffer.rtf");
    }

    private void CancelRecception(object sender, EventArgs args)
    {
        var clientsView = sender as ClientsView; 
        string json = JsonConvert.SerializeObject(new Appointment(clientsView.IdAppointment, _currentDate, TimeOnly.Parse(clientsView.Time), clientsView.OMS, _idDoctor, 4));
        ApiHelper.Put(json, "Appointments", clientsView.IdAppointment);
        ClearPage();
    }

    private void ClearPage()
    {
        AnalyzeRTB = new();
        ResearchRTB = new();
        AnalyzeIsActive = false;
        ResearchIsActive = false;
        Complaints = string.Empty;
        Osmotor = String.Empty;
        PrimaryDiagnosis = string.Empty;
        SecondaryDiagnosis = string.Empty;
        Recommendations = string.Empty;
        NameAnalyze = string.Empty;
        NameResearch = string.Empty;
        PatientFIO = string.Empty;
        PatientOMS = string.Empty;
        SelectSpeciality = null;
        _image = null;
        Directions.Clear();
        ReloadPage(this, EventArgs.Empty);
    }

    private async Task AddAppointmentDocument(int? id)
    {
        Document doc = new Document();
        Section section = doc.AddSection();
        
        Paragraph title = section.AddParagraph();
        title.AppendText($"Дата: {_currentDate}\nПолис ОМС: {PatientOMS}\nМедицинское учреждение: ГБУЗ ДКЦ 1 ДЗМ\nСпециализация: {Specialities[_idDoctor-1].NameSpecialities}\nФИО: {_doctor.Surname} {_doctor.FirstName} {_doctor.Patronymic}\n");

        Paragraph inspection = section.AddParagraph();
        inspection.AppendText($"Осмотр {Specialities[_idDoctor-1].NameSpecialities}а");
        inspection.Format.HorizontalAlignment = HorizontalAlignment.Center;
        inspection.BreakCharacterFormat.Bold = true;

        Table table = section.AddTable();
        int rowCount = 4;
        int colCount = 2;
        table.ResetCells(rowCount, colCount);

        table.Rows[0].Cells[0].AddParagraph().AppendText("Жалобы: ");
        if (!string.IsNullOrEmpty(Complaints))
            table.Rows[0].Cells[1].AddParagraph().AppendText($"предъявляет\n{Complaints}\n");
        else
            table.Rows[0].Cells[1].AddParagraph().AppendText("не предъявляет");

        table.Rows[1].Cells[0].AddParagraph().AppendText("Общий осмтр:");
        table.Rows[1].Cells[1].AddParagraph().AppendText($"{Osmotor}");

        table.Rows[2].Cells[0].AddParagraph().AppendText("Основной диагноз:");
        table.Rows[2].Cells[1].AddParagraph().AppendText($"{PrimaryDiagnosis}. {SecondaryDiagnosis}");

        table.Rows[3].Cells[0].AddParagraph().AppendText("Рекомендация:");
        table.Rows[3].Cells[1].AddParagraph().AppendText($"{Recommendations}");

        table.Rows[0].Cells[0].CellFormat.Borders.Top.BorderType = BorderStyle.Single;
        table.Rows[0].Cells[1].CellFormat.Borders.Top.BorderType = BorderStyle.Single;
        table.Rows[1].Cells[0].CellFormat.Borders.Top.BorderType = BorderStyle.Single;
        table.Rows[1].Cells[1].CellFormat.Borders.Top.BorderType = BorderStyle.Single;
        table.Rows[2].Cells[0].CellFormat.Borders.Top.BorderType = BorderStyle.Single;
        table.Rows[2].Cells[1].CellFormat.Borders.Top.BorderType = BorderStyle.Single;
        table.Rows[3].Cells[0].CellFormat.Borders.Top.BorderType = BorderStyle.Single;
        table.Rows[3].Cells[1].CellFormat.Borders.Top.BorderType = BorderStyle.Single;

        doc.SaveToFile("receptionBuffer.rtf", FileFormat.Rtf);
        string reception = File.ReadAllText("receptionBuffer.rtf");
        File.Delete("receptionBuffer.rtf");
        
        var appointmentDocument = new AppointmentDocument(id, reception, $"Осмотр {Specialities[_idDoctor-1].NameSpecialities}");
        string jsonAppointmentDocument = JsonConvert.SerializeObject(appointmentDocument);
        ApiHelper.Post(jsonAppointmentDocument, "AppointmentDocuments");
    }

    private async Task AddAnalysDocument(int? id)
    {
        var range = new TextRange(AnalyzeRTB.ContentStart, AnalyzeRTB.ContentEnd);
        var fs = new FileStream("analyzeBuffer.rtf", FileMode.Create);
        range.Save(fs, DataFormats.Rtf);
        fs.Close();
        string analyze =  File.ReadAllText("analyzeBuffer.rtf");
        var analysDocument = new AnalysDocument((int)id, analyze, NameAnalyze);
        string jsonAnalysDocument = JsonConvert.SerializeObject(analysDocument);
        ApiHelper.Post(jsonAnalysDocument, "AnalysDocuments");
    }

    private async Task AddResearchDocuments(int? id)
    {
        var range = new TextRange(ResearchRTB.ContentStart, ResearchRTB.ContentEnd);
        var fs = new FileStream("researchBuffer.rtf", FileMode.Create);
        range.Save(fs, DataFormats.Rtf);
        fs.Close();
        string research =  File.ReadAllText("researchBuffer.rtf");
        string jsonResearchDocument = JsonConvert.SerializeObject(new ResearchDocument(id, research, NameResearch, _image));
        ApiHelper.Post(jsonResearchDocument, "ResearchDocuments");
    }

    private async Task AddPatientDirections()
    {
        foreach (var direction in Directions)
        {
            string json = JsonConvert.SerializeObject(new Direction(direction.IdSpeciality, Convert.ToInt64(PatientOMS)));
            ApiHelper.Post(json, "Directions");
        }
    }
}