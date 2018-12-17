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
                var news = (from x in context.News_Messages where x.DeletedAt == null  select x).ToList();


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
            if (MessageBox.Show("Wilt u dit nieuws bericht definitief verwijderen?",
                    "Bevestig verwijdering",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) !=
                MessageBoxResult.Yes)
                return;
            nmc.Delete_NewsMessage((int)b.Tag);
            Switcher.Switch(new NewsMessageList());
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
