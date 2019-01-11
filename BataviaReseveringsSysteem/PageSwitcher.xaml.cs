using BataviaReseveringsSysteem.Database;
using System.Windows.Controls;
using System.Windows.Input;
using Controllers;
using Views;

namespace ScreenSwitcher
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PageSwitcher
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
                if (context.Database.Exists() && new UserController().DataBaseContainsUndeletedManagementUser())
                    Switcher.Switch(new LoginView());
                else Switcher.Switch(new Register());
            }
        }

        public void SwitcherContentCanvas()
        {
            switcherCanvas = new Canvas
            {
                Width = 1024,
                Height = 768
            };
            switcherGrid.Children.Add(switcherCanvas);
        }
        // Verwijderen van het Menu balk
        public void DeleteMenu() => switcherGrid.Children.Remove(menuCanvas);
        // Aanmaken vnan 
        public void MenuMaker()
        {
            var NavigationView = new NavigationView();
            menuCanvas.Children.Add(NavigationView);
            switcherGrid.Children.Add(menuCanvas);
        }

        // het daad werkelijk switchen van 1 pagina naar het andere gebeurt hier
        public void Navigate(UserControl nextPage)
        {
            switcherCanvas.Children.Clear(); // leeg de canvas
            switcherCanvas.Children.Add(nextPage); // voeg nieuwe pagina toe aan canvas
        }

        private void PageSwitcher_OnMouseDown(object sender, MouseButtonEventArgs e) => DetectClick?.Invoke(sender, e);

        public static event MouseButtonEventHandler DetectClick;
    }
}
