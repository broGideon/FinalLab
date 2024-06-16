
using System.Windows;

namespace FinalLab.View.Cards
{
    public partial class ReferralView
    {
        private string _speciality;
        public string Speciality
        {
            get => _speciality;
            set
            {
                _speciality = "Направление к специалисту: " + value;
            }
        }
        public int IdSpeciality;
        public event EventHandler DeleteSpeciality;
        public ReferralView(int idSpeciality, string speciality)
        {
            InitializeComponent();
            DataContext = this;
            Speciality = speciality;
            IdSpeciality = idSpeciality;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteSpeciality(this, EventArgs.Empty);
        }
    }
}
