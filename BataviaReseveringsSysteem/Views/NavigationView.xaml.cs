using ScreenSwitcher;
using System.Windows;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Database;
using System.Linq;
using BataviaReseveringsSysteem.Views;

namespace Views

{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView : UserControl
    {
        public NavigationView()
        {
            InitializeComponent();
            Roles();
            FillName();
            Dashboard.navigationview = this;
        }

        private void Roles()
        {
            using (DataBase context = new DataBase())
            {
                //Maakt een lijst van alle rollen, van de ingelogde user
                var RolID = (from data in context.User_Roles
                             where data.UserID == LoginView.UserId
                             select data.RoleID).ToList();

                // Als de user een reperateur en bestuur is: 
                if (RolID.Contains(1) || RolID.Contains(5))
                {
                    //Mag die boten inzien en toevoegen
                    SeeBoatsBtn.IsEnabled = true;
                    AddBoatsBtn.IsEnabled = true;
                    AddNewsMessageBtn.IsEnabled = true;
                    SeeNewsBtn.IsEnabled = true;
                }
                else
                {
                    //Mag die boten inzien en toevoegen
                    SeeBoatsBtn.IsEnabled = false;
                    AddBoatsBtn.IsEnabled = false;
                }
                // Als de user een coach is: 
                if (RolID.Contains(2))
                {

                }
                // Als de user een wedstrijd commisaris is: 
                if (RolID.Contains(3))
                {

                }
                // Als de user een examinator is: 
                if (RolID.Contains(4) || RolID.Contains(5) )
                {
                    //Mag die diploma's toevoegen
                    SeeUserDiplomasBtn.IsEnabled = true;
                    SeeBoatDiplomasBtn.IsEnabled = true;
                }
                else
                {

                    SeeUserDiplomasBtn.IsEnabled = false;
                    SeeBoatDiplomasBtn.IsEnabled = false;
                }

				// Als de user een bestuur is: 

                if (RolID.Contains(5))
                {
                    //Mag die users inzien en toevoegen
                    SeeUsersBtn.IsEnabled = true;
                    AddUsersBtn.IsEnabled = true;
                   
                }
                else
                {

                   //Mag die users inzien en toevoegen
                    SeeUsersBtn.IsEnabled = false;
                    AddUsersBtn.IsEnabled = false;


                }


            }
        }
        public void MakeAddReservationInvisible(bool boolean)
        {
            MakeReservationsBtn.IsEnabled = boolean;
        }

        //De naam van de user wordt getoond in de menubalk, rechtsboven
        private void FillName()
        {
            using (DataBase context = new DataBase())
            {

                var loggedUser = (from data in context.Users
                                  where data.UserID == LoginView.UserId
                                  select data).Single();
                NameLabel.Content = loggedUser.Firstname + " " + loggedUser.Middlename + " " + loggedUser.Lastname;

            }
        }

        //Als de user uitlogt, dan wordt de menubalk verwijderd en wordt de user terugverwezen naar LoginView
        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            LoginView.UserId = 0;
            Switcher.DeleteMenu();
            Switcher.Switch(new LoginView());

        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new EditUser(LoginView.UserId));

        }

        private void MakeReservationsBtn_Click(object sender, RoutedEventArgs e)
        {
            var reserveWindow = new ReserveWindow();
            Switcher.Switch(reserveWindow);
            reserveWindow.Populate();

        }

        private void SeeReservationsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void ReportDamageaBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDamage());
        }

        private void seeDamageListBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDamageList());
        }


        private void SeeBoatsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatList());
        }

        private void AddBoatsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        private void SeeUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserList());
        }

        private void AddUsersBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Register());
        }

        private void SeeUserDiplomasBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserDiplomaList());
        }
        private void SeeBoatDiplomasBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDiplomaList());

        }

        private void SeeNewsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new NewsMessageList());
        }

        private void AddNewsMessageBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddNewsMessage());
        }
    }
}
