using System.Windows.Controls;

namespace Jarvis.ProjectJarvis.Views.UserControls.Information
{
    public partial class UpdatePointUC : UserControl
    {
        public UpdatePointUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }
    }
}