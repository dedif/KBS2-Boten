using BataviaReseveringsSysteem.Controllers;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Views;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for NewsMessageList.xaml
    /// </summary>
    public partial class NewsMessageList : UserControl
    {
        public static DataGrid DataGrid;

        private NewsMessageController nmc = new NewsMessageController();
        public NewsMessageList()
        {
            InitializeComponent();
            Load();
        }
        private void Load()
        {

            //DataUserList.ItemsSource = context.Users.ToList();
            using (DataBase context = new DataBase())
            {
                var news = (from x in context.News_Messages join u in context.Users on x.UserID equals u.UserID where x.DeletedAt == null  select new {NewsMessageID = x.NewsMessageID, CreatedAt = x.CreatedAt, Message = x.Message, Title = x.Title, Firstname = u.Firstname, Middlename = u.Middlename, Lastname = u.Lastname }).ToList();


                DataNewsMessageList.ItemsSource = news;


                DataGrid = DataNewsMessageList;
            }

        }

        private void AddBoatButton(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        void ButtonDelete(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("Weet u zeker dat u dit nieuwsbericht wilt verwijderen?", "Bevestiging verwijdering", System.Windows.Forms.MessageBoxButtons.YesNo, 30000);

            switch (Succes)
            {
                case System.Windows.Forms.DialogResult.Yes:

                    nmc.Delete_NewsMessage((int)b.Tag);
                    Switcher.Switch(new NewsMessageList());

                    break;


                case System.Windows.Forms.DialogResult.No:

                    break;
            }
            
        }




        void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditNewsMessage((int)b.Tag));

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (DataBase context = new DataBase())
            {

                DataNewsMessageList.ItemsSource = (from x in context.News_Messages
                                                   where (x.NewsMessageID.ToString() == Search.Text || x.Title.Contains(Search.Text) || x.CreatedAt.ToString() == Search.Text ) && x.DeletedAt == null
                                                   select x).ToList();


                DataGrid = DataNewsMessageList;
            }
        }
    }
}
