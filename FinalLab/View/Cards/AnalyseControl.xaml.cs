using System.Windows;

namespace FinalLab.View.Cards;

public partial class AnalyseControl
{
    public string Address;

    public int IdAppointment;

    public AnalyseControl(string analyseName, string date, int idAppointment, string address)
    {
        InitializeComponent();
        DataContext = this;
        AnalyseName = analyseName;
        Date = date;
        IdAppointment = idAppointment;
        Address = address;
    }

    public string AnalyseName { get; set; }

    public string Date { get; set; }

    public event EventHandler Click;

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Click(this, EventArgs.Empty);
    }
}