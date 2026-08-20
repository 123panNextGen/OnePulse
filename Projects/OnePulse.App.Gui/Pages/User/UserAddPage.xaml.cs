using Microsoft.UI.Xaml.Controls;
using OnePulse.App.Gui.Services;
using OnePulse.App.Gui.ViewModels;

namespace OnePulse.App.Gui.Pages.User
{
    public sealed partial class UserAddPage : Page
    {
        public UserAddPageViewModel ViewModel { get; } = new();

        public UserAddPage()
        {
            InitializeComponent();
        }

        private void PasswordInputBox_PasswordChanged(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Password = PasswordInputBox.Password;
        }

        private void CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void LoginButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (ViewModel.UserName.Trim().Length <= 0)
            {
                NotificationService.Show("错误", "用户名为空", InfoBarSeverity.Error);
                return;
            }

        }
    }
}