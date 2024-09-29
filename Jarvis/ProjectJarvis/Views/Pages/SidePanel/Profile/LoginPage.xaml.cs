using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using  Jarvis.ProjectJarvis.Services.Authentificate;
using  Jarvis.ProjectJarvis.Model;
using Newtonsoft.Json;

namespace Jarvis.ProjectJarvis.Views.Pages.SidePanel.Profile;

public partial class LoginPage : Page
{
    
    private readonly IAuthentificateService authService = new AuthentificateService();
    public LoginPage()
    {
        InitializeComponent();
    }

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {

        var login = LoginBox.Text!;
        var password = PassBox.Text!;

        try
        {
            var session = authService.Login(new AuthUserDto
            {
                Username = login,
                Password = password
            });
            File.WriteAllText("session.json", JsonConvert.SerializeObject(session));
            NavigationService!.Navigate(new ProfilePage());
        }
        catch (Exception exception)
        {
            ErrorBlock.Text = exception.Message;
        }

    }

    private void ToRegister_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.Navigate(new Uri(
                @"ProjectJarvis/Views/Pages/SidePanel/Profile/RegistrationPage.xaml", UriKind.Relative
            )
        );
    }
}