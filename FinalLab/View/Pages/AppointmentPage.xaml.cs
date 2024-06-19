using FinalLab.ViewModel.Pages;

namespace FinalLab.View.Pages;

public partial class AppointmentPage
{
    private readonly AppointmentViewModel _viewModel;

    public AppointmentPage()
    {
        InitializeComponent();
        _viewModel = new AppointmentViewModel();
        DataContext = _viewModel;
        RTB.Document = _viewModel.RTB;
    }
}