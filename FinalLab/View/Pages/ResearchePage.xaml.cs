using FinalLab.View.Cards;
using FinalLab.ViewModel.Pages;

namespace FinalLab.View.Pages;

public partial class ResearchePage
{
    private ResearcheViewModel _viewModel;
    public ResearchePage()
    {
        InitializeComponent();
        _viewModel = new ResearcheViewModel();
        DataContext = _viewModel;
        RTB.Document = _viewModel.RTB;
    }
}