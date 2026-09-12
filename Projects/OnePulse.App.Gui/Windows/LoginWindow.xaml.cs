using Microsoft.UI.Xaml;
using OnePulse.App.Gui.Services;
using WinUIEx;

namespace OnePulse.App.Gui.Windows
{
    public sealed partial class LoginWindow
    {
        public LoginWindow()
        {
            // TitleBar
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            InitializeComponent();
            SetWindowMinSize();

            Activated += (s, e) =>
            {
                NotificationService.Initialize(NotificationQueue);
            };
        }

        private void SetWindowMinSize()
        {
            WindowManager manager = WindowManager.Get(this);
            manager.PersistenceId = "LoginWindow";
            manager.MinWidth = 800;
            manager.MinHeight = 600;
        }

        private void AppTitleBar_BackRequested(Microsoft.UI.Xaml.Controls.TitleBar sender, object args)
        {
            if (RootFrame.CanGoBack)
            {
                RootFrame.GoBack();
            }
        }
    }
}
