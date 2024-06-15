using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Windows;

public partial class DortorPage : Page
{
    public DortorPage(DoctorViewModel _viewModel)
    {
        InitializeComponent();

        DataContext = _viewModel;
        
        SpecialitiesComboBox.DisplayMemberPath = "NameSpecialities";
        AnalyzeRTB.Document = _viewModel.AnalyzeRTB;
        ResearchRTB.Document = _viewModel.ResearchRTB;
    }
}