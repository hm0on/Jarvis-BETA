using System.Windows;
using System.Windows.Controls;

namespace Jarvis.ProjectJarvis.Services.GetWorkingButtons;

public static class UpdateWorkingButtonsService
{
    public delegate void ButtonAction(Button button);
    public static void UpdateWorkingButtons(string gridName, string buttonName, ButtonAction buttonAction)
    {
        var mainWindow = Application.Current.MainWindow;
        var controlFrame = mainWindow.FindName(gridName) as Grid;
        var button = controlFrame.FindName(buttonName) as Button;
        buttonAction.Invoke(button);
    }
}
