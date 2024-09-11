using System.Windows.Controls;

namespace Jarvis.ProjectJarvis.Views.UserControls.Information;

public partial class CurrencyUC : UserControl
{
    public CurrencyUC()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string Title { get; set; }

    public string Exchange { get; set; }
}