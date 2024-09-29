using Jarvis.ProjectJarvis.Model;
using Jarvis.ProjectJarvis.Services.Authentificate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Jarvis.ProjectJarvis.Views.Pages.SidePanel.Profile
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private IAuthentificateService _authentificateService;
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text != string.Empty && PassBox.Text != string.Empty) 
            {
                var user = new Model.AuthUserDto()
                {
                    Username = LoginBox.Text,
                    Password = PassBox.Text
                };

                try
                {
                    _authentificateService = new AuthentificateService();
                    _authentificateService.Login(user);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "401")
                        RegistrationProcess(user);
                    else
                    {
                        MessageBox.Show(ex.Message);
                        new Exception(ex.Message);
                    }
                }
            }
        }

        private void RegistrationProcess(Model.AuthUserDto user)
        {
            try
            {
                SessionDto sessionDto = _authentificateService.Register(user);
                File.WriteAllText("session.json", JsonConvert.SerializeObject(sessionDto));
                NavigationService!.Navigate(
                    new Uri(@"ProjectJarvis/Views/Pages/SidePanel/Profile/ProfilePage.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


    }
}
