using FinalLab.ViewModel.Pages;

namespace FinalLab.View.Pages;

public partial class AnalyzePage
{
    private AnalyseViewModel _viewModel;
    public AnalyzePage()
    {
        InitializeComponent();
        _viewModel = new AnalyseViewModel();
        DataContext = _viewModel;
        RTB.Document = _viewModel.RTB;
    }
}