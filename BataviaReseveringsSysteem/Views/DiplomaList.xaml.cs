using BataviaReseveringsSysteem.Database;
using Models;
using ScreenSwitcher;
using System;
using System.Collections;
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
    /// Interaction logic for CertificateList.xaml
    /// </summary>
    public partial class DiplomaList : UserControl
    {
        public static DataGrid DataGrid;

        private DataBase context = new DataBase();
        private DataBaseController dbc = new DataBaseController();
        public DiplomaList()
        {
            InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;

            Load();
        }

        //public static IEnumerable getQueryTable()
        //{
        //    using (DataBase cont = new DataBase())
        //    {
        //        var dataList = (from d in cont.MemberDiplomas
        //                        join c in cont.Users
        //                        on d.PersonID equals c.PersonID
        //                        select new { c.PersonID, Firstname = c.Firstname, Middlename = c.Middlename, Lastname = c.Lastname, S1 = d.DiplomaID.Equals(1), S2 = d.DiplomaID.Equals(2), S3 = d.DiplomaID.Equals(3), P1 = d.DiplomaID.Equals(4), P2 = d.DiplomaID.Equals(5) });

        //        return dataList;
        //    }
        //} 

        private void Load()
        {

            //DataUserList.ItemsSource = context.Users.ToList();
            //var user = (from x in context.Users
            //            join
            //            b in context.MemberDiplomas on x.PersonID equals b.PersonID into cuc
            //            from z in cuc
            //            where x.Deleted_at == null 
            //        select new { PersonID = z.PersonID, Firstname = x.Firstname, Middlename = x.Middlename, Lastname = x.Lastname, S1 = z.DiplomaID.Equals(1), S2= z.DiplomaID.Equals(2), S3 = z.DiplomaID.Equals(3), P1 = z.DiplomaID.Equals(4), P2 = z.DiplomaID.Equals(5) }).ToList();

            //var query = from u in context.Users
            //            join p in context.MemberDiplomas on u.PersonID equals p.PersonID 

            //            select new
            //            {
            //                PersonID = u.PersonID,
            //                Firstname = u.Firstname,
            //                Middlename = u.Middlename,
            //                Lastname = u.Lastname,
            //                S1 = p.DiplomaID.Equals(1),
            //                S2 = p.DiplomaID.Equals(2),
            //                S3 = p.DiplomaID.Equals(3),
            //                P1 = p.DiplomaID.Equals(4),
            //                P2 = p.DiplomaID.Equals(5)
            //            };

            //var Join = (from u in context.Users
            //           join b in context.MemberRoles
            //           on u.PersonID equals b.PersonID
            //           where u.PersonID == b.PersonID 
            //           select new { PersonID = u.PersonID, Firstname = u.Firstname, Middlename = u.Middlename, Lastname = u.Lastname }).ToList();



            using (DataBase cont = new DataBase())
            {
                var omar = (from b in context.MemberDiplomas
                            join c in context.Users
                            on b.PersonID equals c.PersonID
                            into userArticles
                            where b.Deleted_at == null select userArticles).ToList();

                var userArticless = (from b in context.MemberDiplomas
                                   join c in context.Users
                                   on b.PersonID equals c.PersonID
                                   into userArticles
                                   where b.Deleted_at == null
                                   from ua in userArticles.DefaultIfEmpty()
                                   select new { ua.PersonID, Firstname = ua.Firstname, Middlename = ua.Middlename, Lastname = ua.Lastname, S1 = b.DiplomaID.Equals(1), S2 = b.DiplomaID.Equals(2), S3 = b.DiplomaID.Equals(3), P1 = b.DiplomaID.Equals(4), P2 = b.DiplomaID.Equals(5) }).ToList();
                Console.WriteLine(omar);

                var users = (from u in context.Users select u).ToList();
                var MemberDiplomas = (from u in context.MemberDiplomas select u).ToList();

                
                foreach (User u in users) {
                    var DiplomaPerUser = (
                        from d in context.MemberDiplomas
                        where d.PersonID == u.PersonID
                        select d.MemberDiplomaID).ToList();
                    DataUserList.ItemsSource = DiplomaPerUser;
                }
      



                //var Join = (from u in context.Users.Join(b in context.MemberDiplomas on u.PersonID equals b.PersonID).Where(u.p)


                //var dl = (from d in cont.MemberDiplomas
                //                join c in cont.Users
                //                on d.PersonID equals c.PersonID where d.Deleted_at == null
                //                select new { c.PersonID, Firstname = c.Firstname, Middlename = c.Middlename, Lastname = c.Lastname, S1 = d.DiplomaID.Equals(1), S2 = d.DiplomaID.Equals(2), S3 = d.DiplomaID.Equals(3), P1 = d.DiplomaID.Equals(4), P2 = d.DiplomaID.Equals(5) }).ToList();




                //DataUserList.ItemsSource = DiplomaPerUser;
           

            DataGrid = DataUserList;
            }
            //DataView.DataBind();
        }

        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Switcher.Switch(new EditDiplomaView((int)b.Tag));
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

            DataUserList.ItemsSource = (from x in context.Users
                                        where x.PersonID.ToString() == Search.Text || x.Firstname.Contains(Search.Text) || x.Lastname.Contains(Search.Text) || x.City.Contains(Search.Text) || x.Address.Contains(Search.Text) || x.City.Contains(Search.Text) || x.Zipcode.Contains(Search.Text) || x.Email.Contains(Search.Text) || x.Phonenumber.Contains(Search.Text) || x.Birthday.Day.ToString() == Search.Text || x.Birthday.Month.ToString() == Search.Text || x.Birthday.Year.ToString() == Search.Text && x.Deleted_at == null
                                        select x).ToList();


            DataGrid = DataUserList;
        }
    }
}
