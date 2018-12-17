using BataviaReseveringsSysteem.Controllers;
using BataviaReseveringsSysteem.Database;
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

                MessageBoxResult Succes = MessageBox.Show(
                    "Het nieuws bericht is succesvol aangepast",
                    "Melding",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                switch (Succes)
                {
                    case MessageBoxResult.OK:
                        nmc.Update_NewsMessage(EditNewsMessageID,TitleBox.Text, NewsMessageBox.Text);
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
