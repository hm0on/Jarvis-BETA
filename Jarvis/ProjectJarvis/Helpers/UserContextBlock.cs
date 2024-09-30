using Jarvis.ProjectJarvis.Model;
using Jarvis.ProjectJarvis.Services.Authentificate;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Button = System.Windows.Controls.Button;

namespace Jarvis.ProjectJarvis.Helpers;

public static class UserContextBlock
{
    private static void BlockUserContext()
    {
        if (AccountActiveFlag() == false)
        {
            var buttonList = //todo
            ChangeUserContext(buttonList);
            return;
        }
        else
        {
            return;
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

    private static void ChangeUserContext(List<Button> buttonList)
    {
        foreach (var button in buttonList)
        {
            button.IsEnabled = false;
            button.Foreground = new SolidColorBrush(Colors.Red);
        }
    }
}
