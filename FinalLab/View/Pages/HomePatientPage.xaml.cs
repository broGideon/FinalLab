using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Pages;

public partial class HomePatientPage : Page
{
    public HomePatientPage(PatientViewModel _viewModel)
    {
        InitializeComponent();
        DataContext = _viewModel;
    }
}