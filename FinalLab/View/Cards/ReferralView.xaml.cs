using System.Windows;

namespace FinalLab.View.Cards;

public partial class ReferralView
{
    private string _speciality;
    public int IdSpeciality;

    public ReferralView(int idSpeciality, string speciality)
    {
        InitializeComponent();
        DataContext = this;
        Speciality = speciality;
        IdSpeciality = idSpeciality;
    }

    public string Speciality
    {
        get => _speciality;
        set => _speciality = "Направление к специалисту: " + value;
    }

    public event EventHandler DeleteSpeciality;

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        DeleteSpeciality(this, EventArgs.Empty);
    }
}