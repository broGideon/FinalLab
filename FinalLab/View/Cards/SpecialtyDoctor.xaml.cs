using System.Windows;

namespace FinalLab.View.Cards;
public partial class SpecialtyDoctor
{
    private string _imagePath;

    public string ImagePath
    {
        get => _imagePath; 
        set => _imagePath = $"../../../Model/Images/{value}.png";
    }
    
    public string NameRole { get; set; }

    public event EventHandler Click;

    public int IdSpeciality;

    public SpecialtyDoctor(string imagePath, string nameRole, int idSpeciality)
    {
        InitializeComponent();
        DataContext = this;
        ImagePath = imagePath;
        NameRole = nameRole;
        IdSpeciality = idSpeciality;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Click(this, EventArgs.Empty);
    }
}

