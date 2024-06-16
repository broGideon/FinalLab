using System.Collections.ObjectModel;
using System.Windows;

namespace FinalLab.View.Cards;

public partial class Data
{
    public string MonthAndYear { get; set; }
    
    public ObservableCollection<Appointments> ElementCurrents { get; set; }
    public ObservableCollection<RecordsArchive> ElementArchives { get; set; }
    public Data(string monthAndYear, ObservableCollection<Appointments> elementCurrents)
    {
        InitializeComponent();
        DataContext = this;
        MonthAndYear = monthAndYear;
        ElementCurrents = elementCurrents;
        MainGrid.Children.Remove(MainGrid.Children[1]);
    }
    
    public Data(string monthAndYear, ObservableCollection<RecordsArchive> elementArchives)
    {
        InitializeComponent();
        DataContext = this;
        MonthAndYear = monthAndYear;
        ElementArchives = elementArchives;
        MainGrid.Children.Remove(MainGrid.Children[2]);
    }
}
