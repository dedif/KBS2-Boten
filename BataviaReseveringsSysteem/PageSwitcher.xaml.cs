using BataviaReseveringsSysteem.Views;
using System.Windows;
using System.Windows.Controls;
using Views;

namespace ScreenSwitcher
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PageSwitcher : Window
    {
        public Canvas switcherCanvas;
        public MenuView MenuView { get; set; }

        public PageSwitcher()
        {
            InitializeComponent();
            MenuMaker();
            SwitcherContentCanvas();
            Switcher.pageSwitcher = this;
            Switcher.Switch(new ReserveWindow());
        }

        public void SwitcherContentCanvas()
        {
            switcherCanvas = new Canvas
            {
                Width = 1920,
                Height = 1080,
                Margin = new Thickness(0, 100, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            switcherGrid.Children.Add(switcherCanvas);
        }

        public void MenuMaker()
        {
            MenuView = new MenuView();
            Canvas menuCanvas = new Canvas
            {
                Width = 1920,
                Height = 80,
                Margin = new Thickness(0, 25, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            menuCanvas.Children.Add(MenuView);
            switcherGrid.Children.Add(menuCanvas);

        public PageSwitcher()
        {
            InitializeComponent();
            Switcher.pageSwitcher = this;
            Switcher.Switch(new LoginView());

            // toevoegen diploma's
            //using (DataBase context = new DataBase())
            //{
            //    Diploma s1 = new Diploma(1, "s1");
            //    Diploma s2 = new Diploma(2, "s2");
            //    Diploma s3 = new Diploma(3, "s3");
            //    Diploma b1 = new Diploma(4, "b1");
            //    Diploma b2 = new Diploma(5, "b2");
            //    Diploma b3 = new Diploma(6, "b3");
            //    Diploma p1 = new Diploma(7, "p1");
            //    Diploma p2 = new Diploma(8, "p2");

            //    context.Diplomas.Add(s1);
            //    context.Diplomas.Add(s2);
            //    context.Diplomas.Add(s3);
            //    context.Diplomas.Add(b1);
            //    context.Diplomas.Add(b2);
            //    context.Diplomas.Add(b3);
            //    context.Diplomas.Add(p1);
            //    context.Diplomas.Add(p2);

            //    context.SaveChanges();
            //}


        }


        public void Navigate(UserControl nextPage)
        {
            this.switcherCanvas.Children.Clear();
            this.switcherCanvas.Children.Add(nextPage);

            this.Content = nextPage;
        }
    }
}
