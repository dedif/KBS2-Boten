using System.Windows;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using Views;
using System.Linq;

namespace BataviaReseveringsSysteem.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            using (DataBase context = new DataBase())
            {

                if (LoginView.UserId != 0 )
                {
                    //var loggedUser = (from data in context.Users
                    //                  where data.UserID == LoginView.UserId
                    //                  select data).Single();



                    var rolName = (from data in context.User_Roles
                                   where data.UserID == LoginView.UserId 
                                   select data.RoleID).ToList();

                    foreach(var r in rolName)
                    {
                        System.Console.WriteLine(r);
                    }

                    //reparateur
                    if (rolName.Contains(1))
                    {
                      

                    }
                    //coach
                    if (rolName.Contains(2))
                    {
                    

                    }
                    //wedstrijd commissaris
                    if (rolName.Contains(3))
                    {
                       
                    }
                    //examinator
                    if (rolName.Contains(4))
                    {
                        Diploma.Visibility = Visibility.Visible;
                      

                    }
                    //admin
                    if (rolName.Contains(5))
                    {
                        Diploma.Visibility = Visibility.Visible;
                        BoatList.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                  
                }
            }

        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        private void AfschrijvingDoen_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ReserveWindow());
        }

        private void AfschrijvingenInzien_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void BotenBekijken_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatList());
        }

        private void BotenToevoegen_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        private void SchadeMelden_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDamage());
        }

        private void SchadeInzien_Click(object sender, RoutedEventArgs e)
        {
            //Switcher.Switch(new BoatDamageList());
        }

        private void DiplomaList_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new DiplomaList());
        }
    }
}
