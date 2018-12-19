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

                  
                    TitleBox.Content = $"{newsMessage.Title} {newsMessage.CreatedAt}";
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
