using System.Windows;

namespace FinalLab.View.Cards;
public partial class RecordsArchive
{
    public string NameDoctor { get; set; }
    public string FIO { get; set; }
    public string Day { get; set; }
    public string Address { get; set; }
    
    public int IdDoctor;

    public int IdAppointment;
    
    public event EventHandler Repeat;
    public event EventHandler Delete;
    public RecordsArchive(string nameDoctor, string fio, string day, string address,  int idDoctor, int idAppointment)
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
    
    private void RepeatClick(object sender, RoutedEventArgs e)
    {
        Repeat(this, EventArgs.Empty);
    }

    private void DeleteClick(object sender, RoutedEventArgs e)
    {
        Delete(this, EventArgs.Empty);
    }
}
