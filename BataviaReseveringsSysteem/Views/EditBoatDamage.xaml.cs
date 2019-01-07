using BataviaReseveringsSysteem.Database;
using BataviaReseveringsSysteem.Views;
using Controllers;
using Models;
using ScreenSwitcher;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    /// <summary>
    /// Interaction logic for EditBoatDamage.xaml
    /// </summary>
    public partial class EditBoatDamage : UserControl
    {
        private string reserved = null;
        private int DamageID;
        private BoatController bc = new BoatController();
        public EditBoatDamage(int damageID)
        {
            InitializeComponent();
            
            TimeOfOccupyForFix.SelectedDate = DateTime.Now;
            TimeOfFix.SelectedDate = DateTime.Now;
            using (DataBase context = new DataBase())
            {
                

                DamageID = damageID;
                var boat = (from data in context.Damages
                            join b in context.Boats on data.BoatID equals b.BoatID
                            where data.DamageID == DamageID
                            select new { b.Name, data.Status, data.Description, data.TimeOfOccupyForFix, data.TimeOfFix }).First();

                SetBlackOutDates(boat.Name);

                NameBoatLabel.Content = boat.Name;

                if (boat.Status.Equals("Lichte schade"))
                {
                    LightDamageRadioButton.IsChecked = true;
                }
                if (boat.Status.Equals("Zware schade"))
                {
                    HeavyDamageRadioButton.IsChecked = true;
                }
                if (boat.Status.Equals("Geen schade"))
                {
                    NoDamageRadioButton.IsChecked = true;
                }
                if (TimeOfOccupyForFix.SelectedDate == TimeOfFix.SelectedDate)
                {
                    Label.Visibility = Visibility.Hidden;
                }
                textboxLabel.Content = boat.Description;
                if (boat.TimeOfOccupyForFix != null)
                {
                    TimeOfOccupyForFix.SelectedDate = boat.TimeOfOccupyForFix;
                }
                if (boat.TimeOfFix != null)
                {
                    TimeOfFix.SelectedDate = boat.TimeOfFix;
                }
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Dashboard());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
              using (DataBase context = new DataBase()) {

                var Reservations = (from data in context.Reservations
                                                where data.Boat.Name == NameBoatLabel.Content.ToString()
                                                where data.Deleted == null
                                                select data).ToList();

                var Boat = (from data in context.Boats
                            where data.Name == NameBoatLabel.Content.ToString() && data.DeletedAt == null
                            select data).Single();

                string status = null;
                if (LightDamageRadioButton.IsChecked == true)
                {
                    status = "Lichte schade";
                }
                else if (HeavyDamageRadioButton.IsChecked == true)
                {
                    status = "Zware schade";
                    Boat.Broken = true;
                    //voor elke reservering met zware schade wordt deleted vadaag
                    foreach (Reservation r in Reservations)
                    {
                        r.Deleted = DateTime.Now;
                    }
                }
                else if (NoDamageRadioButton.IsChecked == true)
                {
                    status = "Geen schade";

                }
                var Damage = (from data in context.Damages
                              join a in context.Boats
                              on data.BoatID equals a.BoatID
                              where data.Description == textboxLabel.Content.ToString()
                              select data).Single();

                            
                            


                            

                            bool reservationDate = false;
                                var ReservationDates = (from data in context.Reservations
                                                        join a in context.Boats
                                                        on data.BoatID equals a.BoatID
                                                        where a.Name == NameBoatLabel.Content.ToString()
                                                        where data.Deleted == null
                                                        where data.End >= DateTime.Now
                                                        select data.End).ToList();

                foreach (DateTime reservation in ReservationDates)
                {

                    if (reservation.DayOfYear >= TimeOfOccupyForFix.SelectedDate.Value.DayOfYear && reservation.DayOfYear <= TimeOfFix.SelectedDate.Value.DayOfYear)
                    {
                        reservationDate = true;
                    }
                }

                    if (reservationDate == false)
                    {
                        CheckDate();
                       
                        if (TimeOfOccupyForFix.SelectedDate.Value <= TimeOfFix.SelectedDate.Value)
                        {//kijkt of reservering al is gereserveerd
                         //AlreadyReserved(NameBoatLabel.Content.ToString());


                        System.Windows.Forms.DialogResult Succes = System.Windows.Forms.MessageBoxEx.Show("Weet u zeker dat u de schade van deze boot wilt aanpassen?", "Bevestiging bewerking", System.Windows.Forms.MessageBoxButtons.YesNo, 30000);

                        switch (Succes)
                        {


                            case System.Windows.Forms.DialogResult.Yes:

                                bc.UpdateBoatDamage(Damage.DamageID, textboxLabel.Content.ToString(), TimeOfOccupyForFix.SelectedDate.Value, TimeOfFix.SelectedDate.Value, status);
                                Switcher.Switch(new BoatDamageList());

                                break;


                            case System.Windows.Forms.DialogResult.No:

                                break;
                        }
                    }
                }
            }
        }

        private void HeavyDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Visible;
        }

        private void NoDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Hidden;
        }

        private void LightDamageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            NotificationLabel.Visibility = Visibility.Hidden;
        }
        //deze methode kijkt of je de beschrijving hebt ingevuld. de boot kan nooit leeg zijn.
        public bool IsEmpty(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageLabel.Visibility = Visibility.Visible;
                return true;
            }
            else
            {
                return false;
            }
        }
        //kijkt of de boot gereserveerd is
        public void AlreadyReserved(string boat)
        {
            using (DataBase context = new DataBase())
            {
                var Reservation = (from data in context.Reservations
                                   select data.Boat.Name).ToList();
                if (Reservation.Contains(boat))
                {
                    reserved = " Deze boot is in de toekomst gereserveerd.";
                }
            }
        }

        private void SetBlackOutDates(string boatName)
        {
            using (DataBase context = new DataBase())
            {
                var dateEnd = (from data in context.Reservations
                                    join a in context.Boats
                                    on data.BoatID equals a.BoatID
                                    where a.Name == boatName
                                    where data.Deleted == null
                                    select data.End).FirstOrDefault();
                
                int date2 = dateEnd.DayOfYear;

                int dateNow = DateTime.Now.DayOfYear;

                var DateReservations = (from data in context.Reservations
                                        join a in context.Boats
                                        on data.BoatID equals a.BoatID
                                        where a.Name == boatName
                                        where data.Deleted == null
                                        where date2 > dateNow
                                        select data.Start).ToList();
                
                foreach (DateTime date in DateReservations) {
                        TimeOfOccupyForFix.BlackoutDates.Add(new CalendarDateRange(date));
                        TimeOfFix.BlackoutDates.Add(new CalendarDateRange(date));
                        DateDamageFix.BlackoutDates.Add(new CalendarDateRange(date));
                    
                }
                

            }
        }
        

        //TimeOfFix
        private void ClickDate(object sender, SelectionChangedEventArgs e)
        {
            // vult de dates
            DateTime timeOfAccupyForFix = DateTime.Now;
            
            if (TimeOfOccupyForFix.SelectedDate != null)
            {
                timeOfAccupyForFix = TimeOfOccupyForFix.SelectedDate.Value;
            }
            DateTime timeOfFix = DateTime.Now;
            
            if (TimeOfFix.SelectedDate != null)
            {
                timeOfFix = TimeOfFix.SelectedDate.Value;
            }
            // maakt database
            using (DataBase context = new DataBase()) {

                Label.Visibility = Visibility.Hidden;
                //geeft een melding als de data niet kloppen
                CheckDate ();
                // kijkt of de data kloppen
                if (timeOfAccupyForFix < timeOfFix)
                {
                    Label.Visibility = Visibility.Hidden;
                    //variablele
                        int i = 1;
                    
                        var ReservationDate = (from data in context.Reservations
                                               join a in context.Boats
                                               on data.BoatID equals a.BoatID
                                               where a.Name == NameBoatLabel.Content.ToString()
                                               where data.Deleted == null
                                               where data.End >= DateTime.Now
                                               select data).ToList();
                    // kijkt of er een reservering is tussen de data 
                    foreach (var date in ReservationDate)
                    {
                        if (date.End.DayOfYear >= timeOfAccupyForFix.DayOfYear && date.End.DayOfYear <= timeOfFix.DayOfYear)
                        {
                            i = 2;
                        }
                    }

                    if (i == 2)
                    {
                        Label.Content = "De boot is gereserveerd";
                        Label.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Label.Visibility = Visibility.Hidden;
                    }
                    if (i == 1)
                    {
                        
                        TimeOfFix.SelectedDate = TimeOfFix.SelectedDate;
                        TimeAddToCalender();
                        DateDamageFix.SelectedDates.Add(TimeOfFix.SelectedDate.Value);
                    }
                    
                } 
            }
        }

        private void ClickDates(object sender, SelectionChangedEventArgs e)
        {
            //vult de dates            
            DateTime timeOfAccupyForFix = DateTime.Now;
            if (TimeOfOccupyForFix.SelectedDate != null)
            {
               timeOfAccupyForFix = TimeOfOccupyForFix.SelectedDate.Value;
            }
            DateTime timeOfFix = DateTime.Now;
            
            if (TimeOfFix.SelectedDate != null)
            {
                timeOfFix = TimeOfFix.SelectedDate.Value;
            }
            Label.Visibility = Visibility.Hidden;
            using (DataBase context = new DataBase()) {

                CheckDate();
                
                var ReservationDate = (from data in context.Reservations
                                       join a in context.Boats
                                       on data.BoatID equals a.BoatID
                                       where a.Name == NameBoatLabel.Content.ToString()
                                       where data.Deleted == null
                                       where data.End >= DateTime.Now
                                       select data).ToList();
                int i = 1;
                foreach (var date in ReservationDate)
                {
                    if (date.End.DayOfYear >= timeOfAccupyForFix.DayOfYear && date.End.DayOfYear <= timeOfFix.DayOfYear)
                    {
                        i = 2;
                    }
                }

                if (i == 2)
                {
                    Label.Content = "De boot is gereserveerd";
                    Label.Visibility = Visibility.Visible;
                }
                else
                {
                    Label.Visibility = Visibility.Hidden;
                }
                if( i == 1)
                {
                    //Label.Visibility = Visibility.Hidden;
                    TimeOfOccupyForFix.SelectedDate = TimeOfOccupyForFix.SelectedDate;
                    TimeAddToCalender();
                    DateDamageFix.SelectedDates.Add(TimeOfOccupyForFix.SelectedDate.Value);
                }
            }
        }

        public void CheckDate()
        {
            DateTime timeOfAccupyForFix = DateTime.Now;
            if (TimeOfOccupyForFix.SelectedDate != null)
            {
                timeOfAccupyForFix = TimeOfOccupyForFix.SelectedDate.Value;
            }
            DateTime timeOfFix = DateTime.Now;
            if (TimeOfFix.SelectedDate != null)
            {
                timeOfFix = TimeOfFix.SelectedDate.Value;
            }
            if (timeOfAccupyForFix > timeOfFix ){
                Label.Content = "De data kloppen niet";
                Label.Visibility = Visibility.Visible;
            }
        }

       public void TimeAddToCalender()
        {
            if(DateDamageFix.SelectedDates.Count == 2)
            {   
                DateDamageFix.SelectedDates.RemoveAt(0);
                DateDamageFix.SelectedDates.RemoveAt(0);
            }
       }

        
    }
}
