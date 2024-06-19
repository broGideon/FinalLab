using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinalLab.View.Cards;

public partial class ClientsView
{
    public int IdAppointment;

    public long OMS;

    public ClientsView(string FIO, string time, long OMS, int idAppointment)
    {
        InitializeComponent();
        DataContext = this;
        this.FIO = FIO;
        Time = time;
        this.OMS = OMS;
        IdAppointment = idAppointment;
    }

    public string FIO { get; set; }

    public string Time { get; set; }

    public event EventHandler StartReception;
    public event EventHandler CancelRecception;

    private void Start(object sender, RoutedEventArgs e)
    {
        StartReception(this, EventArgs.Empty);
    }

    private void CancelClick(object sender, RoutedEventArgs e)
    {
        Cancel();
    }

    public void Cancel()
    {
        ContainerButton.Children.Clear();
        TextBlock textBlock = new Wpf.Ui.Controls.TextBlock();
        textBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e8eaed"));
        textBlock.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryForeground");
        textBlock.TextAlignment = TextAlignment.Center;
        textBlock.Text = "Запись завершена";
        ContainerButton.ColumnDefinitions.Clear();
        ContainerButton.Children.Add(textBlock);
        CancelRecception(this, EventArgs.Empty);
    }
}