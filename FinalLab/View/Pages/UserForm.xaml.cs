using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Pages;

public partial class UserForm : Page
{
    public UserForm(AdministratorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}