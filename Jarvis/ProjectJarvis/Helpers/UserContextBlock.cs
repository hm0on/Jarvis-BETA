using Jarvis.ProjectJarvis.Model;
using Jarvis.ProjectJarvis.Services.Authentificate;
using Jarvis.ProjectJarvis.Services.GetWorkingButtons;
using System;
using System.Windows.Media;
using Button = System.Windows.Controls.Button;
using Jarvis.Project.Services.VoskSpeechRecognition;

namespace Jarvis.ProjectJarvis.Helpers;

public static class UserContextBlock
{
    public static bool VoskRecordingFlag;

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
            var authenticateService = new AuthentificateService();
            var user = authenticateService.GetMe(session);
            var dateTime = user.ExpiredTime;

            return !(dateTime == null | dateTime < DateTime.UtcNow);
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
    
    private static void UnBlockUserContent(Button button)
    {
        button.IsEnabled = true;
        button.Background  = new SolidColorBrush(Colors.Transparent);
    }
}
