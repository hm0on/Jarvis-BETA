using Jarvis.ProjectJarvis.Model;
using Jarvis.ProjectJarvis.Services.Authentificate;
using Jarvis.ProjectJarvis.Services.GetWorkingButtons;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Jarvis.ProjectJarvis.Services.GetWorkingButtons;
using Button = System.Windows.Controls.Button;
using Jarvis.Assets;

namespace Jarvis.ProjectJarvis.Helpers;

public static class UserContextBlock
{
    public static bool BlockUserContext()
    {
        if (AccountActiveFlag() == false)
        {
            var buttonList = ButtonListClass.ButtonList;
            foreach (var button in buttonList)
            {
                UpdateWorkingButtonsService.UpdateWorkingButtons("ControlFrame", button, ChangeUserContext);
            }
            return false;
        }
    }
    
    
    public static void UnBlockUserContext()
    {
        if (AccountActiveFlag() == false)
        {
            var buttonList = ButtonListClass.ButtonList;
            foreach (var button in buttonList)
            {
                UpdateWorkingButtonsService.UpdateWorkingButtons("ControlFrame", button, UnBlockUserContext);
            }
        }
        else
        {
            return true;
        }
    }


    private static bool AccountActiveFlag()
    {
        try
        {
            var session = SessionDto.GetSession();
            var _authentificateService = new AuthentificateService();
            var user = _authentificateService.GetMe(session);
            var dateTime = user.ExpiredTime;

            if (dateTime < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }
        catch
        {

            return false;
        }
    }

    private static void ChangeUserContext(Button button)
    {
        button.IsEnabled = false;
        button.Background = new SolidColorBrush(Color.FromArgb(255, 150, 0, 0));
    }
    
    private static void UnBlockUserContext(Button button)
    {
        button.IsEnabled = true;
        ResourceDictionary resourceDictionary = new ResourceDictionary();
        resourceDictionary.Source = new Uri("ProjectJarvis/Views/StylesDictionary/ButtonsDictionary.xaml", UriKind.Relative);
        button.Style = (Style)resourceDictionary["Main.SideButton"];
    }
}
