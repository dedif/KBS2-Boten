using BataviaReseveringsSysteem.Controllers;
using ScreenSwitcher;
using System.Windows;
using System.Windows.Controls;
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

        // voeg een nieuwsbericht toe
        private void SaveNewsMessage_Click(object sender, RoutedEventArgs e)
        {
            // controleer of de titel en het bericht niet leeg zijn
            if (nmc.WhiteCheck(TitleBox.Text, NewsMessageBox.Text) == true)
            {

                NotificationLabel.Content = nmc.Notification();

                //popup box en voeg daarna het bericht toe in de database
                System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("Uw bericht is geplaatst", "Gelukt!", System.Windows.Forms.MessageBoxButtons.OK, 30000);

                switch (Succes)
                {
                    case System.Windows.Forms.DialogResult.OK:
                        nmc.Add_NewsMessage(LoginView.UserId,TitleBox.Text, NewsMessageBox.Text);
                        Switcher.Switch(new NewsMessageList());
                        break;

                }
            }
            else
            {
                //error label
                NotificationLabel.Content = nmc.Notification();

            }
        }

        // annuleer knop
        private void CancelNewsMessage_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new NewsMessageList());
        }
    }
}
