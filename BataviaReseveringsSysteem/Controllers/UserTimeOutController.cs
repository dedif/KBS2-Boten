using System;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;
using Views;
using ScreenSwitcher;

namespace BataviaReseveringsSysteem.Controllers
{
    // deze klasse zorgt ervoor dat je automatisch na 90 seconden wordt uitgelocht. Dit gebeurd als je voor 90 seconden geen activiteiten op de applicatie zijn geweest.
    class UserTimeOutController
    {

        private readonly DispatcherTimer _activityTimer;
        private System.Windows.Point _inactiveMousePosition = new System.Windows.Point(0, 0);

        private IInputElement _inputElement;
        private int _idleTime = 300;

        public event EventHandler IsIdle;

        public UserTimeOutController(IInputElement inputElement, int idleTime)
        {
            _inputElement = inputElement;
            InputManager.Current.PreProcessInput += OnActivity;
            _activityTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(idleTime), IsEnabled = true };
            _activityTimer.Tick += OnInactivity;
        }

        public void ChangeIdleTime(int newIdleTime)
        {
            _idleTime = newIdleTime;

            _activityTimer.Stop();
            _activityTimer.Interval = TimeSpan.FromSeconds(newIdleTime);
            _activityTimer.Start();
        }

        void OnInactivity(object sender, EventArgs e)
        {
            _inactiveMousePosition = Mouse.GetPosition(_inputElement);
            _activityTimer.Stop();
            IsIdle?.Invoke(this, new EventArgs());
            // logout, haal het menu weg en ga naar de login pagina als er geen activiteit is geweest voor 90 seconden
            LoginView.UserId = 0;
            Switcher.DeleteMenu();
            Switcher.Switch(new LoginView());

        }

        void OnActivity(object sender, PreProcessInputEventArgs e)
        {
            InputEventArgs inputEventArgs = e.StagingItem.Input;

            if (inputEventArgs is MouseEventArgs || inputEventArgs is KeyboardEventArgs)
            {
                if (e.StagingItem.Input is MouseEventArgs)
                {
                    MouseEventArgs mouseEventArgs = (MouseEventArgs)e.StagingItem.Input;

                    // no button is pressed and the position is still the same as the application became inactive
                    if (mouseEventArgs.LeftButton == MouseButtonState.Released &&
                        mouseEventArgs.RightButton == MouseButtonState.Released &&
                        mouseEventArgs.MiddleButton == MouseButtonState.Released &&
                        mouseEventArgs.XButton1 == MouseButtonState.Released &&
                        mouseEventArgs.XButton2 == MouseButtonState.Released &&
                        _inactiveMousePosition == mouseEventArgs.GetPosition(_inputElement))
                        return;
                }

                _activityTimer.Stop();
                _activityTimer.Start();
            }
        }
    }
}
   
