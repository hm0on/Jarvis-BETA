using Jarvis.ProjectJarvis.Model;
using Jarvis.ProjectJarvis.Services.Authentificate;
using Jarvis.ProjectJarvis.Services.GetWorkingButtons;
using System;
using System.Windows;
using System.Windows.Media;
using Button = System.Windows.Controls.Button;
using Jarvis.Assets;
using Jarvis.Project.Services.VoskSpeechRecognition;

namespace Jarvis.ProjectJarvis.Helpers;

public static class UserContextBlock
{
    public static bool VoskRecordingFlag = false;

    public static bool BlockUserContext()
    {
        if (!AccountActiveFlag())
        {
            var buttonList = ButtonListClass.ButtonList;
            foreach (var button in buttonList)
            {
                UpdateWorkingButtonsService.UpdateWorkingButtons("ControlFrame", button, BlockUserContent);
            }

            VoskSpeechRecognition.waveIn?.StopRecording();
            VoskRecordingFlag = false;
            return false;
        }

        return true;
    }
    
    
    public static void UnBlockUserContext()
    {
        if (AccountActiveFlag())
        {
            var buttonList = ButtonListClass.ButtonList;
            foreach (var button in buttonList)
            {
                UpdateWorkingButtonsService.UpdateWorkingButtons("ControlFrame", button, UnBlockUserContent);
            }
            if (!VoskRecordingFlag)
            {
                VoskSpeechRecognition.waveIn?.StartRecording();
                VoskRecordingFlag = true;
            }
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

            if (dateTime == null | dateTime < DateTime.UtcNow)
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

    private static void BlockUserContent(Button button)
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
