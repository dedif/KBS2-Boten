using BataviaReseveringsSysteem.Database;
using Models;
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
using System.Windows.Shapes;
using Views;

namespace ScreenSwitcher
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PageSwitcher : Window
    {
        public PageSwitcher()
        {
            InitializeComponent();
            Switcher.pageSwitcher = this;
            Switcher.Switch(new Register());

            //toevoegen diploma's
            using (DataBase context = new DataBase())
            {
                Diploma s1 = new Diploma(1, "s1");
                Diploma s2 = new Diploma(2, "s2");
                Diploma s3 = new Diploma(3, "s3");
                Diploma b1 = new Diploma(4, "b1");
                Diploma b2 = new Diploma(5, "b2");
                Diploma b3 = new Diploma(6, "b3");
                Diploma p1 = new Diploma(7, "p1");
                Diploma p2 = new Diploma(8, "p2");

                context.Diplomas.Add(s1);
                context.Diplomas.Add(s2);
                context.Diplomas.Add(s3);
                context.Diplomas.Add(b1);
                context.Diplomas.Add(b2);
                context.Diplomas.Add(b3);
                context.Diplomas.Add(p1);
                context.Diplomas.Add(p2);

                context.SaveChanges();
            }

        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }
    }
}
