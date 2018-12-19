using BataviaReseveringsSysteem.Database;
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
        Canvas menuCanvas = new Canvas();

        public PageSwitcher()
        {
            InitializeComponent();
            SwitcherContentCanvas();
            Switcher.pageSwitcher = this;
            using (var context = new DataBase())
            {
                if (context.Database.Exists()) Switcher.Switch(new LoginView());
                else Switcher.Switch(new Register());
            }
            
        }

        public void SwitcherContentCanvas()
        {
            switcherCanvas = new Canvas
            {

                Width = 1024,
                Height = 768,
            };
            switcherGrid.Children.Add(switcherCanvas);
        }

        public void DeleteMenu()
        {
            switcherGrid.Children.Remove(menuCanvas);
        }

        public void MenuMaker()
        {
            NavigationView NavigationView = new NavigationView();
            menuCanvas.Children.Add(NavigationView);
            switcherGrid.Children.Add(menuCanvas);
        }


        public void Navigate(UserControl nextPage)
        {
            switcherCanvas.Children.Clear();
            switcherCanvas.Children.Add(nextPage);
        }
    }
}
