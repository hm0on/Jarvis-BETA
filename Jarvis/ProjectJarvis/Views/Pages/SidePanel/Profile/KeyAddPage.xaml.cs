using Jarvis.ProjectJarvis.Model;
using Jarvis.ProjectJarvis.Services.Authentificate;
using NAudio.CoreAudioApi.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using Jarvis.ProjectJarvis.Helpers;

namespace Jarvis.ProjectJarvis.Views.Pages.SidePanel.Profile;

public partial class KeyAddPage : Page
{
    private IAuthentificateService _authentificateService;

    public KeyAddPage()
    {
        InitializeComponent();
    }

    private void Back_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.Navigate(new Uri(
                @"ProjectJarvis/Views/Pages/SidePanel/Profile/ProfilePage.xaml", UriKind.Relative
            )
        );
    }

    private void RegisterKey_OnClick(object sender, RoutedEventArgs e)
    {
        _authentificateService = new AuthentificateService();
        try
        {
            var session = SessionDto.GetSession();

            if (KeyText.Text == string.Empty || !Guid.TryParse(KeyText.Text, out var result))
            {
                MessageBox.Show("Неправильный формат Токена");
                return;
            }
        
            _authentificateService.AddKey(session, result.ToString());
            UserContextBlock.UnBlockUserContext();
            NavigationService!.Navigate(new Uri(
                @"ProjectJarvis/Views/Pages/SidePanel/Profile/ProfilePage.xaml", UriKind.Relative
            ));
        }
        catch (Exception ex)
        {
            new Exception(ex.Message);
        }
    }
}