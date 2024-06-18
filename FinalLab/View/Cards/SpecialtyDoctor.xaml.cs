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

    public SpecialtyDoctor(string imagePath, string nameRole)
    {
        InitializeComponent();
        DataContext = this;
        ImagePath = imagePath;
        NameRole = nameRole;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}

