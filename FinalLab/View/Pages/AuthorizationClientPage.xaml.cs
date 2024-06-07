using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Pages;

public partial class AuthorizationClientPage : Page
{
    public AuthorizationClientPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}