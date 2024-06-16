using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Windows;

public partial class DortorPage
{
    private DoctorViewModel _viewModel;
    public DortorPage(DoctorViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        SpecialitiesComboBox.DisplayMemberPath = "NameSpecialities";
        AnalyzeRTB.Document = _viewModel.AnalyzeRTB;
        ResearchRTB.Document = _viewModel.ResearchRTB;
        _viewModel.ReloadPage += (_, _) => Reload();
    }

    private void Reload()
    {
        AnalyzeRTB.Document = _viewModel.AnalyzeRTB;
        ResearchRTB.Document = _viewModel.ResearchRTB;
    }
}