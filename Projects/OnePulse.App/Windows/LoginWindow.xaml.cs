using Microsoft.UI.Xaml;

namespace OnePulse.App.Windows;

public sealed partial class LoginWindow : Window
{
    public LoginWindow()
    {
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        InitializeComponent();
    }
}
