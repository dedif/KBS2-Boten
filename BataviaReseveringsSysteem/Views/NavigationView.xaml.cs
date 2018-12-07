﻿using ScreenSwitcher;
using Views;
using System.Windows;
using System.Windows.Controls;
using BataviaReseveringsSysteem.Database;
using System.Linq;
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

        }

        private void Roles()
        {
            using (DataBase context = new DataBase())
            {
                //Maakt een lijst van alle rollen, van de ingelogde user
                var RolID = (from data in context.MemberRoles
                             where data.PersonID == LoginView.UserId
                             select data.RoleID).ToList();

                // Als de user een reperateur is: 
                if (RolID.Contains(1))
                {

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
                if (RolID.Contains(4))
                {
                    //Mag die diploma's toevoegen
                    SeeDiplomasBtn.IsEnabled = true;
                }
                // Als de user een besstuur is: 
                if (RolID.Contains(5))
                {
                    //Mag die boten inzien en toevoegen
                    SeeBoatsBtn.IsEnabled = true;
                    AddBoatsBtn.IsEnabled = true;
                    //Mag die users inzien en toevoegen
                    SeeUsersBtn.IsEnabled = true;
                    AddUsersBtn.IsEnabled = true;
                   
                }



            }
        }

        //De naam van de user wordt getoond in de menubalk, rechtsboven
        private void FillName()
        {
            using (DataBase context = new DataBase())
            {

                var loggedUser = (from data in context.Users
                                  where data.PersonID == LoginView.UserId
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

        private void MakeReservationsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ReserveWindow());
        }

        private void SeeReservationsBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void ReportDamageaBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDamage());
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

        private void SeeDiplomasBtn_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new DiplomaList());
        }
    }
}
