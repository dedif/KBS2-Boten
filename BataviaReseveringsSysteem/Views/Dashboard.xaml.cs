using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using Models;
using BataviaReseveringsSysteem.Database;
using ScreenSwitcher;
using System;

namespace Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {

        int YLeft = 50;
        int YRight = 50;
        int Count = 0;
        int MaxReservationUser = 2;
        //Deze lijsten, bevatten alle buttens en labels
        List<Label> LabelList = new List<Label>();
        List<Button> ButtonList = new List<Button>();
        DataBase context = new DataBase();
        public Dashboard()
        {
            InitializeComponent();
            GridDashboard.VerticalAlignment = VerticalAlignment.Top;
            GridDashboard.Margin = new Thickness(50, 0, 50, 20);
            NameLabel.Content = LoginView.LoggedUser.Firstname + " " + LoginView.LoggedUser.Lastname;
           
                var rol = (from data in context.MemberRoles
                           where data.PersonID == LoginView.LoggedUser.PersonID
                           select data.RoleID).ToList();

                if (rol.Contains(6))
                {
                    MaxReservationUser = 2;
                    //AddBoatButton.Visibility = Visibility.Visible;
                    //UserListButton.Visibility = Visibility.Visible;
                }

                if (rol.Contains(3))
                {
                    MaxReservationUser = 8;
                }
          
                if (rol.Contains(4))
                {
                    MaxReservationUser = int.MaxValue;
                }

                if (rol.Contains(5))
                {
                    MaxReservationUser = int.MaxValue;
                }


                //De reservaties van de gebruiker worden met deze methode getoond op het scherm
                ShowReservations();

            
        }

        public void ShowReservations()
        {
            using (DataBase context = new DataBase())
            {
                //Als de gebruiker nog geen afschrijvingen heeft, dan komt dit op het scherm te staan. 
                if (context.Reservations.Where(i => i.Deleted == null).Count() == 0)
                {
                    NoReservationLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    NoReservationLabel.Visibility = Visibility.Hidden;
                }

                if (context.Reservations.Where(i => i.Deleted == null).Count() >= MaxReservationUser)
                {
                    MaxReservations.Visibility = Visibility.Visible;
                    AddReservationButton.IsEnabled = false;
                }
                else
                {
                    MaxReservations.Visibility = Visibility.Hidden;
                    AddReservationButton.IsEnabled = true;
                }

                var OrderedReservations = (from data in context.Reservations
                                           where data.Deleted == null 
                                           orderby data.Start
                                           select data).ToList();

                foreach (Reservation r in OrderedReservations)
                {
                    if (Count % 2 == 0)
                    {

                        //Dit is voor de label aan de linkerkant van de twee rijen
                        Label l = new Label()
                        {
                            Content = ReservationContent(r),
                            Margin = new Thickness(20, YLeft, 50, 50),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l);
                        Button deleteButton = AddDeleteButton(20, YLeft + 130, r.ReservationID);
                        Button changeButton = AddChangeButton(20, YLeft + 170);
                        ButtonList.Add(deleteButton);
                        ButtonList.Add(changeButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        GridDashboard.Children.Add(l);
                        GridDashboard.Children.Add(deleteButton);


                        YLeft = YLeft + 300;
                    }
                    else if (Count % 2 != 0)
                    {
                        //Hiermee maak je een label
                        Label l2 = new Label()
                        {
                            Content = ReservationContent(r),
                            Margin = new Thickness(500, YRight, 50, 50),
                            FontSize = 16,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                        };
                        LabelList.Add(l2);
                        Button deleteButton = AddDeleteButton(500, YRight + 130, r.ReservationID);
                        Button changeButton = AddChangeButton(500, YRight + 170);
                        ButtonList.Add(deleteButton);
                        ButtonList.Add(changeButton);

                        //Dit voegt de label en knoppen toe aan het scherm
                        GridDashboard.Children.Add(l2);
                        GridDashboard.Children.Add(deleteButton);


                        YRight = YRight + 300;
                    }


                    Count++;
                }
            }
        }

        //Deze methode vult de labels van de huidige reservaties
        public string ReservationContent(Reservation reservation)
        {
            using (DataBase context = new DataBase())
            {

                var ReservationBoatID = (
                    from r in context.Reservations
                    where r.ReservationID == reservation.ReservationID
                    select r.Boat.BoatID).Single();

                var Name =
                    (from boat in context.Boats
                     where boat.BoatID == ReservationBoatID
                     select boat.Name).Single();

                var StartDate =
                  (from r in context.Reservations
                   where r.ReservationID == reservation.ReservationID
                   select r.Start).Single();

                var EndDate =
             (from r in context.Reservations
              where r.ReservationID == reservation.ReservationID
              select r.End).Single();

                var Duration = EndDate - StartDate;

                string Minutes = StartDate.Minute.ToString();
                if (StartDate.Minute < 10)
                {
                    Minutes = "0" + Minutes;
                }

                string content;
                content = "Naam : " + Name;
                content += "\nBegintijd: " + StartDate.Hour + ":" + Minutes;
                content += "\nDuur: " + Duration.Hours + ":" + Duration.Minutes;
                content += "\nDatum: " + StartDate.Month + "/" + StartDate.Day + "/" + StartDate.Year;

                return content;
            }
        }

        //Deze methode verwijderd de bijbehorende reservatie
        public void DeleteReservation(int id)
        {
            using (DataBase context = new DataBase())
            {
                var Delete = (
                    from r in context.Reservations
                    where r.ReservationID == id
                    select r).Single();
                //De gebruiker krijgt een controle melding.
                MessageBoxResult confirm = MessageBox.Show(
                                "Weet u zeker dat u de volgende afschrijving wilt verwijderen:\n"
                                + ReservationContent(Delete),
                                "Melding",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information);

                //Als de gebruiker de reservering wilt verwijderen.
                if (confirm == MessageBoxResult.Yes)
                {
                    //De reservering wordt uit de database verwijderd. 
                    //context.Reservations.Remove(Delete);

                    Delete.Deleted = DateTime.Now;
                    context.SaveChanges();
                    //Alle oude knoppen en labels worden verwijderd van het scherm.
                    this.DeleteAllControls();
                    YLeft = 50;
                    YRight = 50;
                    Count = 0;
                    //De nieuwe reserveringen worden op het scherm getoond. 
                    ShowReservations();
                }

            }
        }
        //Deze methode verwijderd alle controls
        public void DeleteAllControls()
        {
            for (int i = 0; i < LabelList.Count; i++)
            {
                GridDashboard.Children.Remove(LabelList[i]);
            }
            for (int i = 0; i < ButtonList.Count; i++)
            {
                GridDashboard.Children.Remove(ButtonList[i]);
            }
        }
        private Button AddChangeButton(int x, int y)
        {
            //Er wordt een button aangemaakt. 
            Button Left = new Button()
            {
                Content = "Afschrijving wijzigen",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(x, y, 0, 0),
                Height = 30,
                Width = 160,
                FontSize = 16,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            //De button krijgt een click event
            Left.Click += Change_Click;

            return Left;
        }


        private Button AddDeleteButton(int x, int y, int id)
        {

            Button Right = new Button()
            {
                //Er wordt een button aangemaakt. 
                Content = "Afschrijving annuleren",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(x, y, 0, 0),
                Height = 30,
                Width = 160,
                FontSize = 16,
                Tag = id,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            //De button krijgt een click event
            Right.Click += DeleteButton_Click;


            return Right;
        }




        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            DeleteReservation((int)b.Tag);
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new ReserveWindow());

        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new LoginView());
        }

        private void UserListButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new UserList());
        }

        private void AddBoatButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new AddBoat());
        }

        private void DamageButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new BoatDamage());
        }
    }
}
