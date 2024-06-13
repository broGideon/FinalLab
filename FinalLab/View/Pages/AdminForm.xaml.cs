using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Pages;

public partial class AdminForm : Page
{
    public AdminForm(AdministratorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}