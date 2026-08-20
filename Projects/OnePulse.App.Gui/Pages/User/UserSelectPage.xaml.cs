using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services;
using OnePulse.Pan123.Api.Models;

namespace OnePulse.App.Gui.Pages.User
{
    public sealed partial class UserSelectPage : Page
    {
        internal ObservableCollection<StorageUser> Users = [];
        internal StorageUser? SelectedUser;
        internal LoginManager Manager { get; private set; } = LoginManager.Instance;

        private async void UpdateUserList()
        {
            Users = new ObservableCollection<StorageUser>(Manager.Get.GetUsers());
        }

        public UserSelectPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                UpdateUserList();
            }
            catch (ArgumentNullException) { }
        }

        private async void OnDeleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (SelectedUser == null || SelectedUser.UserName == null)
                return;

            var confirm = new ContentDialog
            {
                Title = "删除用户",
                Content =
                    $"确定删除用户「{SelectedUser.UserName}」？其保存的密码与令牌将一并移除。",
                PrimaryButtonText = "删除",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = XamlRoot,
            };
            if (await confirm.ShowAsync() != ContentDialogResult.Primary)
                return;

            var result = Manager.Delete.DeleteUser(SelectedUser.UserName);
            if (result.Result == ApiResult.Success)
            {
                UpdateUserList();
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

        private void UserInfoListView_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        ) {
            SelectedUser = (StorageUser?)UserInfoListView.SelectedItem;
        }

        private void NewButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserAddPage));
        }
    }
}
