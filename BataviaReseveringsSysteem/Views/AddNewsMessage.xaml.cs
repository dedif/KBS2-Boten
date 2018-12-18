using BataviaReseveringsSysteem.Controllers;
using ScreenSwitcher;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Views;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for AddNewsMessage.xaml
    /// </summary>
    public partial class AddNewsMessage : UserControl
    {
        NewsMessageController nmc = new NewsMessageController();
        public AddNewsMessage()
        {
            InitializeComponent();
        }

        private void SaveNewsMessage_Click(object sender, RoutedEventArgs e)
        {
            if (nmc.WhiteCheck(TitleBox.Text, NewsMessageBox.Text) == true)
            {


              

                NotificationLabel.Content = nmc.Notification();

                MessageBoxResult Succes = MessageBox.Show(
                    "De boot is succesvol opgeslagen",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                switch (Succes)
                {
                    case MessageBoxResult.OK:
                        nmc.Add_NewsMessage(TitleBox.Text, NewsMessageBox.Text);
                        Switcher.Switch(new NewsMessageList());
                        
                        break;

                }
            } else
            {
                NotificationLabel.Content = nmc.Notification();

            }
        }

        

        private void CancelNewsMessage_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new NewsMessageList());
        }
    }
}
