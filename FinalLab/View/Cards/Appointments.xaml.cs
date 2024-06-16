using System.Windows;

namespace FinalLab.View.Cards;

public partial class Appointments
{
    public string NameDoctor { get; set; }
    public string FIO { get; set; }
    public string Day { get; set; }
    public string Address { get; set; }

    public int IdDoctor;

    public int IdAppointment;

    public event EventHandler Move;
    public event EventHandler Delete;
    public Appointments(string nameDoctor, string fio, string day, string address, int idDoctor, int idAppointment)
    {
        InitializeComponent();
        DataContext = this;
        NameDoctor = nameDoctor;
        FIO = fio;
        Day = day;
        Address = address;
        IdAppointment = idAppointment;
        IdDoctor = idDoctor;
    }

    private void MoveClick(object sender, RoutedEventArgs e)
    {
        Move(this, EventArgs.Empty);
    }

    private void DeleteClick(object sender, RoutedEventArgs e)
    {
        Delete(this, EventArgs.Empty);
    }
}
