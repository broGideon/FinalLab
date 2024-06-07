using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Pages;

public partial class AuthorizationDoctorPage : Page
{
    public AuthorizationDoctorPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}