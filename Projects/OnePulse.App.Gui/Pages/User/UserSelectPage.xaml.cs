using Microsoft.UI.Xaml.Controls;
using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services;
using OnePulse.Pan123.Api.Models;
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

        private async void OnDeleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (UserInfoListView.SelectedItem is not StorageUser selected || selected.UserName is null)
                return;

            var confirm = new ContentDialog
            {
                Title = "删除用户",
                Content = $"确定删除用户「{selected.UserName}」？其保存的密码与令牌将一并移除。",
                PrimaryButtonText = "删除",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = XamlRoot,
            };
            if (await confirm.ShowAsync() != ContentDialogResult.Primary)
                return;

            var result = Manager.Delete.DeleteUser(selected.UserName);
            if (result.Result == ApiResult.Success)
            {
                UserInfoListView.ItemsSource = Manager.Get.GetUsers();
            }
            else
            {
                await new ContentDialog
                {
                    Title = "删除失败",
                    Content = result.Message,
                    CloseButtonText = "确定",
                    XamlRoot = XamlRoot,
                }.ShowAsync();
            }
        }
    }
}
