using System.Windows.Controls;

namespace ScreenSwitcher
{
    public static class Switcher
    {
        public static PageSwitcher pageSwitcher;

        public static void Switch(UserControl newPage) => pageSwitcher.Navigate(newPage);

        public static void MenuMaker() => pageSwitcher.MenuMaker();

        public static void DeleteMenu() => pageSwitcher.DeleteMenu();
    }
}
