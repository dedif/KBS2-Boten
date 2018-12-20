using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Views;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for ShowNewsMessage.xaml
    /// </summary>
    public partial class ShowNewsMessage : UserControl
    {
        public ShowNewsMessage(int newsMessageID)
        {
            InitializeComponent();
            using (DataBase context = new DataBase())
            {


                var news = from x in context.News_Messages
                           where x.NewsMessageID == newsMessageID
                           select x;
                
                foreach (var newsMessage in news)
                {
                   
                  
                    // content van de pagina invullen
                    TitleBox.Content = newsMessage.Title;
                    NewsMessageDateLabel.Content = newsMessage.CreatedAt.ToString("dd-MM-yyyy");
                    NewsMessageBox.Text = newsMessage.Message;
                }
                
}
        }

        private void CancelNewsMessage_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }
    }
}
