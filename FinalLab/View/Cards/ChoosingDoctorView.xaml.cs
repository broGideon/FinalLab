using System.Windows;

namespace FinalLab.View.Cards;

public partial class ChoosingDoctorView
{
    public int IdDoctor;

    public ChoosingDoctorView(string fio, string date, string address, int idDoctor)
    {
        InitializeComponent();
        FIO = fio;
        Date = date;
        Address = address;
        IdDoctor = idDoctor;
    }

    public string FIO { get; set; }

    public string Date { get; set; }

    public string Address { get; set; }

    public event EventHandler SelectionDoctor;

    private void Selection(object sender, RoutedEventArgs e)
    {
        SelectionDoctor(this, EventArgs.Empty);
    }
}