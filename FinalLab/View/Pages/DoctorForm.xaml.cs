using System.Windows.Controls;
using FinalLab.ViewModel;

namespace FinalLab.View.Pages;

public partial class DoctorForm : Page
{
    public DoctorForm(AdministratorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}