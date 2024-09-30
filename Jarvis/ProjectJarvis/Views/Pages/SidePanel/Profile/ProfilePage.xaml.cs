using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Jarvis.ProjectJarvis.Services.Authentificate;
using Jarvis.ProjectJarvis.Model;
using Jarvis.ProjectJarvis.Helpers;
namespace Jarvis.ProjectJarvis.Views.Pages.SidePanel.Profile;

public partial class ProfilePage : Page
{
    private readonly IAuthentificateService auth = new AuthentificateService();

    private void LoadUser(SessionDto session)
    {
        if (session != null)
        {
            UserDto user;
            try
            {
                user = auth.GetMe(session);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                NavigationService!.Navigate(
                    new Uri(@"ProjectJarvis/Views/Pages/SidePanel/Profile/LoginPage.xaml", UriKind.Relative));
                return;
            }
            catch (Exception)
            {
                NavigationService!.Navigate(
                    new Uri(@"ProjectJarvis/Views/Pages/SidePanel/Profile/LoginPage.xaml", UriKind.Relative));
                return;
            }

            IdLabel.Content += user.Id.ToString();
            UsernameLabel.Content += user.Username;
            if (user.ExpiredTime == null) SubscribeLabel.Content += " Не активна";
            else SubscribeLabel.Content += " Активна";

            if (user.ExpiredTime != null)
                EndDateLabel.Content += user.ExpiredTime.Value.ToLocalTime().ToString(" dd MMMM yyyy H:m ");
            else
            {
                EndDateLabel.Visibility = Visibility.Collapsed;
            }
        }
    }

    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        try
        {
            var session = SessionDto.GetSession();
            LoadUser(session);
            UserContextBlock.UnBlockUserContext();
        }
        catch (ValidationException)
        {
            NavigationService!.Navigate(new LoginPage());
        }
    }

    private void AddKey_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService!.Navigate(new Uri(
            @"ProjectJarvis/Views/Pages/SidePanel/Profile/KeyAddPage.xaml", UriKind.Relative
            )
        );
    }

    private void Logout_OnClick(object sender, RoutedEventArgs e)
    {
        File.Delete("session.json");
        UserContextBlock.BlockUserContext();
        NavigationService!.Navigate(new Uri(
            @"ProjectJarvis/Views/Pages/SidePanel/Profile/LoginPage.xaml", UriKind.Relative
            )
        );

    }
}