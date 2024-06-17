using System.Windows;
using FinalLab.Model;
using FinalLab.View;
using SecondLibPractice;

namespace FinalLab.ViewModel.Pages;

public class RecordViewModel : BindingHelper
{
    #region MyRegion

    private long _oms;
    public RecordViewModel()
    {
        var window = Application.Current.Windows.OfType<PatientWindow>().FirstOrDefault();
        _oms = (window.PatientsComboBox.SelectedItem as Patient).Oms;
        window.WindowTextBlock.Text = "Записаться на прием";
    }
    
    #endregion
}