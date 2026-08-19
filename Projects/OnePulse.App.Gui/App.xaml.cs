using Microsoft.UI.Xaml;
using OnePulse.App.Gui.Windows;

namespace OnePulse.App.Gui
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new LoginWindow();
            _window.Activate();
        }
    }
}
