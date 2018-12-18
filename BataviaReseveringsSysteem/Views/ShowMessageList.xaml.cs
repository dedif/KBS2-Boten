using BataviaReseveringsSysteem.Database;
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

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for ShowMessageList.xaml
    /// </summary>
    public partial class ShowMessageList : UserControl
    {
        public int ShowNewsMessageID;
        public ShowMessageList(int showNewsMessageID)
        {
            InitializeComponent();
            ShowNewsMessageID = showNewsMessageID;
            using (DataBase context = new DataBase()) {
                var getNewsMessage = (from n in context.News_Messages where n.NewsMessageID == showNewsMessageID select n).Single();

                TitleBox.Content = getNewsMessage.Title;
                CreatedAtBox.Content = getNewsMessage.CreatedAt.ToString();
                NewsMessageBox.Text = getNewsMessage.Message;
            }
        }
    }
}
