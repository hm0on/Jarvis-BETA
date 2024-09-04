using System.Windows.Controls;
using System.Windows.Media;

namespace Jarvis.ProjectJarvis.Views.UserControls.Primary
{
    public partial class AnswerPointUC : UserControl
    {
        public AnswerPointUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Message { get; set; }

        public SolidColorBrush Color { get; set; }
    }
}