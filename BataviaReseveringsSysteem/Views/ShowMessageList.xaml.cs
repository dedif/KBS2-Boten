using BataviaReseveringsSysteem.Database;
using System.Linq;
using System.Windows.Controls;

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
