using System.Windows.Data;
using FinalLab.Model;
using FinalLab.View.Cards;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class HomePatientViewModel : BindingHelper
{
    #region MyRegion

    private List<SpecialtyDoctor> _specialtyDoctorCards = new();

    public List<SpecialtyDoctor> SpecialtyDoctorCards
    {
        get => _specialtyDoctorCards;
        set => SetField(ref _specialtyDoctorCards, value);
    }
    
    

    #endregion
    public HomePatientViewModel()
    {
        List<Speciality>? specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        foreach (var speciality in specialities!)
        {
            SpecialtyDoctor specialtyDoctor = new SpecialtyDoctor(speciality.NumberImage.ToString(), speciality.NameSpecialities);
            SpecialtyDoctorCards.Add(specialtyDoctor);
        }
    }    
}