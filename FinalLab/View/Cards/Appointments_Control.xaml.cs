using System.Windows;

namespace FinalLab.View.Cards;

public partial class Appointments_Control
{
    public int IdAppointment;

    public Appointments_Control(string nameResearch, string fio, string date, string address, int idAppointment)
    {
        InitializeComponent();
        NameResearch = nameResearch;
        FIO = fio;
        Date = date;
        Address = address;
        IdAppointment = idAppointment;
    }

    public string NameResearch { get; set; }

    public string FIO { get; set; }

    public string Date { get; set; }

    public string Address { get; set; }

    public event EventHandler Click;

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Click(this, EventArgs.Empty);
    }
}