using System;
using System.Windows;
using System.Windows.Controls;

namespace Jarvis.ProjectJarvis.Views.Pages.SidePanel.Profile;

public partial class KeyAddPage : Page
{
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
        
    }
}