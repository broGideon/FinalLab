using System.Collections.ObjectModel;
using System.Windows;
using FinalLab.Model;
using FinalLab.View;
using FinalLab.View.Cards;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class RecordViewModel : BindingHelper
{
    #region MyRegion

    private ObservableCollection<SpecialtyDoctor> _specialCards;
    
    public ObservableCollection<SpecialtyDoctor> SpecialCards
    {
        get => _specialCards;
        set => SetField(ref _specialCards, value);
    }
    
    private ObservableCollection<SpecialtyDoctor> _specialitiesCards;
    
    public ObservableCollection<SpecialtyDoctor> SpecialitesCards
    {
        get => _specialitiesCards;
        set => SetField(ref _specialitiesCards, value);
    }
    
    private ObservableCollection<SpecialtyDoctor> _directionsCards;
    
    public ObservableCollection<SpecialtyDoctor> DirectionsCards
    {
        get => _directionsCards;
        set => SetField(ref _directionsCards, value);
    }
    
    private ObservableCollection<SpecialtyDoctor> _purposeCards;
    
    public ObservableCollection<SpecialtyDoctor> PurposeCards
    {
        get => _purposeCards;
        set => SetField(ref _purposeCards, value);
    }

    private long _oms;
    public RecordViewModel()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        _oms = (window.PatientsComboBox.SelectedItem as Patient).Oms;
        window.WindowTextBlock.Text = "Записаться на прием";
        _ = LoadDirectionsCards();
    }
    
    #endregion

    private async Task LoadSpecialsCards()
    {
        
    }

    private async Task LoadSpecialitiesCards()
    {
        
    }

    private async Task LoadDirectionsCards()
    {
        var directions = ApiHelper.Get<List<Direction>>("Directions")!.Where(item => item.Oms == _oms).ToList();
        List<Speciality>? specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        foreach (var item in directions!)
        {
            SpecialtyDoctor specialtyDoctor = new SpecialtyDoctor(specialities![(int)(item.SpecialityId-1)!].NumberImage.ToString(), specialities[(int)(item.SpecialityId-1)!].NameSpecialities);
            DirectionsCards.Add(specialtyDoctor);
        }
    }

    private async Task LoadPurposeCards()
    {
        
    }
}