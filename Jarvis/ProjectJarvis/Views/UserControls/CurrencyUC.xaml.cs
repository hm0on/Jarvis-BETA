using System.Windows.Controls;

namespace Jarvis.ProjectJarvis.Views.UserControls
{
    public partial class CurrencyUC : UserControl
    {
        public CurrencyUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }
        public string Exchange { get; set; }
    }
}