using BataviaReseveringsSysteem.Database;
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
        public NavigationView NavigationView { get; set; }

        public PageSwitcher()
        {
            InitializeComponent();
            

            MenuMaker();
            SwitcherContentCanvas();
            Switcher.pageSwitcher = this;
            Switcher.Switch(new LoginView());
        }

        public void SwitcherContentCanvas()
        {
            switcherCanvas = new Canvas
            {
                Width = 1024,
                Height = 768,
                //Margin = new Thickness(0, 100, 0, 0),
                //HorizontalAlignment = HorizontalAlignment.Left,
                //VerticalAlignment = VerticalAlignment.Top,
            };
            switcherGrid.Children.Add(switcherCanvas);
        }

        public void MenuMaker()
        {
            NavigationView = new NavigationView();
            Canvas menuCanvas = new Canvas();
            menuCanvas.Children.Add(NavigationView);
            Label l1 = new Label();
            l1.Content = "";
            DataBase context = new DataBase();
            foreach (var item in context.Diplomas.ToString())
            {

            }
            switcherGrid.Children.Add(menuCanvas);
        }


        public void Navigate(UserControl nextPage)
        {
            this.switcherCanvas.Children.Clear();
            this.switcherCanvas.Children.Add(nextPage);
        }
    }
}
