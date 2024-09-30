using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Jarvis.ProjectJarvis.Services.GetWorkingButtons;

public static class GetWorkingButtonsService
{
    public static Button GetWorkingButtons(NavigationService navigationService, Window windowToButton, string buttonName)
    {
        var window = navigationService.Content as Window;
        var controlFrameChild = window!.FindName(buttonName) as Button;
        
        return controlFrameChild;
    }
}