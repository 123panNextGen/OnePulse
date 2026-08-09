using Microsoft.UI.Xaml;
using OnePulse.Features.LoginManager.Services;

namespace OnePulse.App.Windows;

public sealed partial class LoginWindow : Window
{
    internal LoginManager Manager { get; private set; } = new();
    internal string AppDataPath = "";

    public LoginWindow()
    {
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);

        InitializeComponent();

        AppDataPath = LoginManager.Instance.AppDataPath;
    }
}
