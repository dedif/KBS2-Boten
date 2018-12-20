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


                System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("Uw bericht is geplaatst", "Gelukt!", System.Windows.Forms.MessageBoxButtons.OK, 30000);

                switch (Succes)
                {
                    case System.Windows.Forms.DialogResult.OK:
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
