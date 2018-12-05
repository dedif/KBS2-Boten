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
            Switcher.Switch(new LoginView());
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
        }


        public void Navigate(UserControl nextPage)
        {
            this.switcherCanvas.Children.Clear();
            this.switcherCanvas.Children.Add(nextPage);
        }
    }
}
