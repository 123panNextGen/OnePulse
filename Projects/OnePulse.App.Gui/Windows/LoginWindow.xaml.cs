using Microsoft.UI.Xaml;

namespace OnePulse.App.Gui.Windows;

public sealed partial class LoginWindow : Window
{
    public LoginWindow()
    {
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        InitializeComponent();
        SetWindowMinSize();
    }

    private void SetWindowMinSize()
    {
        var manager = WinUIEx.WindowManager.Get(this);
        manager.PersistenceId = "LoginWindow";
        manager.MinWidth = 800;
        manager.MinHeight = 600;
    }
}
