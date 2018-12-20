using BataviaReseveringsSysteem.Controllers;
using ScreenSwitcher;
using System.Windows;
using System.Windows.Controls;

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
                    "Uw bericht is geplaatst",
                    "Gelukt!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                switch (Succes)
                {
                    case MessageBoxResult.OK:
                        nmc.Add_NewsMessage(TitleBox.Text, NewsMessageBox.Text);
                        Switcher.Switch(new NewsMessageList());

                        break;
                }
            }
            else
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
