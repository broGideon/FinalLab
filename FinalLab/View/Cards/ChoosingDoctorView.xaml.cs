
using System.Windows;

namespace FinalLab.View.Cards;
public partial class ChoosingDoctorView
{
    public string FIO { get; set; }
    
    public string Date { get; set; }
    
    public string Address { get; set; }

    public int IdDoctor;

    public event EventHandler SelectionDoctor;
    public ChoosingDoctorView(string fio, string date, string address, int idDoctor)
    {
        InitializeComponent();
        FIO = fio;
        Date = date;
        Address = address;
        IdDoctor = idDoctor;
    }

    private void Selection(object sender, RoutedEventArgs e)
    {
        SelectionDoctor(this, EventArgs.Empty);
    }
}
