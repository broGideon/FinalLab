using FinalLab.ViewModel.Pages;

namespace FinalLab.View.Pages;

public partial class RecordPage
{
    public RecordPage()
    {
        InitializeComponent();
        DataContext = new RecordViewModel();
    }
    
}