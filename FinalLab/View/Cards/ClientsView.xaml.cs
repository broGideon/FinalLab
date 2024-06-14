
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FinalLab.View.Cards
{
    public partial class ClientsView
    {
        public string FIO { get; set; }
        
        public string Time { get; set; }

        public long OMS;

        private event EventHandler StartReception;
        
        public ClientsView(string FIO, string time, long OMS)
        {
            InitializeComponent();
            DataContext = this;
            this.FIO = FIO;
            Time = time;
            this.OMS = OMS;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            ContainerButton.Children.Clear();
            TextBlock textBlock = new Wpf.Ui.Controls.TextBlock();
            textBlock.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e8eaed"));
            textBlock.SetResourceReference(TextBlock.ForegroundProperty, "PrimaryForeground");
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Text = "Запись завершена";
            ContainerButton.ColumnDefinitions.Clear();
            ContainerButton.Children.Add(textBlock);
        }
    }
}
