using System.Collections.ObjectModel;
using System.Windows;
using FinalLab.Model;
using FinalLab.View;
using FinalLab.View.Cards;
using FinalLab.View.Pages;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class RecordViewModel : BindingHelper
{
    #region Variables

    private ObservableCollection<SpecialtyDoctor> _specialCards = new();
    
    public ObservableCollection<SpecialtyDoctor> SpecialCards
    {
        get => _specialCards;
        set => SetField(ref _specialCards, value);
    }
    
    private ObservableCollection<SpecialtyDoctor> _specialitiesCards = new();
    
    public ObservableCollection<SpecialtyDoctor> SpecialitesCards
    {
        get => _specialitiesCards;
        set => SetField(ref _specialitiesCards, value);
    }
    
    private ObservableCollection<SpecialtyDoctor> _directionsCards = new();
    
    public ObservableCollection<SpecialtyDoctor> DirectionsCards
    {
        get => _directionsCards;
        set => SetField(ref _directionsCards, value);
    }
    
    private ObservableCollection<SpecialtyDoctor> _purposeCards = new();
    
    public ObservableCollection<SpecialtyDoctor> PurposeCards
    {
        get => _purposeCards;
        set => SetField(ref _purposeCards, value);
    }

    private long _oms;
    
    #endregion

    #region Methods
    
    public RecordViewModel()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        _oms = (window.PatientsComboBox.SelectedItem as Patient).Oms;
        window.WindowTextBlock.Text = "Записаться на прием";
        _ = LoadSpecialsCards();
        _ = LoadSpecialitiesCards();
        _ = LoadDirectionsCards();
        _ = LoadPurposeCards();
    }
    
    private async Task LoadSpecialsCards()
    {
        var orvi = new SpecialtyDoctor("1", "Дежурный врач по ОРВИ", 1);
        orvi.Click += (sender, args) => Recording(sender, args);
        SpecialCards.Add(orvi);
        var covid = new SpecialtyDoctor("covid", "Вакцинация от COVID-19", 1);
        covid.Click += (sender, args) => Recording(sender, args);
        SpecialCards.Add(covid);
    }

    private async Task LoadSpecialitiesCards()
    {
        var specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        foreach (var speciality in specialities)
        {
            SpecialtyDoctor specialtyDoctor = new SpecialtyDoctor(speciality.NumberImage.ToString(), speciality.NameSpecialities, (int)speciality.IdSpeciality!);
            specialtyDoctor.Click += (sender, args) => Recording(sender, args);
            SpecialitesCards.Add(specialtyDoctor);
        }
    }

    private async Task LoadDirectionsCards()
    {
        var directions = ApiHelper.Get<List<Direction>>("Directions")!.Where(item => item.Oms == _oms).ToList();
        var specialities = ApiHelper.Get<List<Speciality>>("Specialities");
        foreach (var item in directions!)
        {
            SpecialtyDoctor specialtyDoctor = new SpecialtyDoctor(specialities![(int)(item.SpecialityId-1)!].NumberImage.ToString(), specialities[(int)(item.SpecialityId-1)!].NameSpecialities, (int)item.SpecialityId!);
            specialtyDoctor.Click += (sender, args) => Recording(sender, args);
            DirectionsCards.Add(specialtyDoctor);
        }
    }

    private async Task LoadPurposeCards()
    {
        var grip = new SpecialtyDoctor("vaccine", "Вакцинация от гриппа", 1);
        grip.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(grip);
        var acuteDisease = new SpecialtyDoctor("emergency", "Острое заболевание", 1);
        acuteDisease.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(acuteDisease);
        var orvi = new SpecialtyDoctor("1", "Дежурный врач ОРВИ", 1);
        orvi.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(orvi);
        var inspection = new SpecialtyDoctor("3", "Осмотр по хронике", 3);
        inspection.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(inspection);
        var document = new SpecialtyDoctor("document", "Оформить документы", 6);
        document.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(document);
        var women = new SpecialtyDoctor("5", "Женская консультация", 5);
        women.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(women);
        var profilactic = new SpecialtyDoctor("10", "Профилактика", 10);
        profilactic.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(profilactic);
        var dantist = new SpecialtyDoctor("6", "Запись к стоматологу", 6);
        dantist.Click += (sender, args) => Recording(sender, args);
        PurposeCards.Add(dantist);
    }

    private void Recording(object sender, EventArgs args)
    {
        var card = sender as SpecialtyDoctor;
        Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault()!.PageFrame.Content = new ChoosingDoctorPage(card!.IdSpeciality);
    }
    
    #endregion
}
