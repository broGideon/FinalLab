namespace FinalLab.View.Cards;

public partial class Data
{
    public string MonthAndYear { get; set; }
    
    public List<Appointments> ElementCurrents { get; set; }
    public List<RecordsArchive> ElementArchives { get; set; }
    public Data(string monthAndYear, List<Appointments> elementCurrents)
    {
        InitializeComponent();
        DataContext = this;
        MonthAndYear = monthAndYear;
        ElementCurrents = elementCurrents;
    }
    
    public Data(string monthAndYear, List<RecordsArchive> elementArchives)
    {
        InitializeComponent();
        DataContext = this;
        MonthAndYear = monthAndYear;
        ElementArchives = elementArchives;
    }
}
