using BataviaReseveringsSysteem.Controllers;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for EditNewsMessage.xaml
    /// </summary>
    public partial class EditNewsMessage : UserControl
    {
        NewsMessageController nmc = new NewsMessageController();
        private int EditNewsMessageID;
        public EditNewsMessage(int editNewsMessageID)
        {
            InitializeComponent();
            EditNewsMessageID = editNewsMessageID;

            using (DataBase context = new DataBase())
            {
                var news = from x in context.News_Messages
                            where x.NewsMessageID == editNewsMessageID
                            select x;

                foreach (var newsMessage in news)
                {
                    TitleBox.Text = newsMessage.Title;
                    NewsMessageBox.Text = newsMessage.Message;
                }
            }
        }
        private void SaveNewsMessage_Click(object sender, RoutedEventArgs e)
        {
            if (nmc.WhiteCheck(TitleBox.Text, NewsMessageBox.Text) == true)
            {
                NotificationLabel.Content = nmc.Notification();


                System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("Het nieuwsbericht is succesvol aangepast", "Succes", System.Windows.Forms.MessageBoxButtons.OK, 30000);

                switch (Succes)
                {
                    case System.Windows.Forms.DialogResult.OK:
                        nmc.Update_NewsMessage(EditNewsMessageID, TitleBox.Text, NewsMessageBox.Text);
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
