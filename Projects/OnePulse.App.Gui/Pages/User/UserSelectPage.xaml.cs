using Microsoft.UI.Xaml.Controls;
using OnePulse.Features.LoginManager.Services;
using System;

namespace OnePulse.App.Gui.Pages.User
{
    public sealed partial class UserSelectPage : Page
    {
        internal LoginManager Manager { get; private set; } = LoginManager.Instance;

        public UserSelectPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                UserInfoListView.ItemsSource = Manager.Get.GetUsers();
            }
            catch (ArgumentNullException)
            {

            }
        }
    }
}
