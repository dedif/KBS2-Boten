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


        public PageSwitcher()
        {
            InitializeComponent();
            SwitcherContentCanvas();
            Switcher.pageSwitcher = this;
            Switcher.Switch(new AddBoat());
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



        public void DeleteMenu()
        {
            switcherGrid.Children.RemoveAt(1);
        }

        public void MenuMaker()
        {
            NavigationView = new NavigationView();
            Canvas menuCanvas = new Canvas();
            menuCanvas.Children.Add(NavigationView);
            switcherGrid.Children.Add(menuCanvas);
        }


        public void Navigate(UserControl nextPage)
        {
            this.switcherCanvas.Children.Clear();
            this.switcherCanvas.Children.Add(nextPage);
        }
    }
}
