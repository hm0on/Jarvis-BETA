using System.Windows.Controls;

namespace Jarvis.ProjectJarvis.Views.UserControls
{
    public partial class BlockUC : UserControl
    {
        public BlockUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}