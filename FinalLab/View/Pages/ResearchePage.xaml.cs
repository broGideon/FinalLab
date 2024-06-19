using FinalLab.ViewModel.Pages;

namespace FinalLab.View.Pages;

public partial class ResearchePage
{
    private readonly ResearcheViewModel _viewModel;

    public ResearchePage()
    {
        InitializeComponent();
        _viewModel = new ResearcheViewModel();
        DataContext = _viewModel;
        RTB.Document = _viewModel.RTB;
    }
}