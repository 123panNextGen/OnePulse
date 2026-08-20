using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace OnePulse.App.Gui.Pages.User
{
    public sealed partial class UserAddPage : Page
    {
        public bool NeedLoginAndEnter { get; set; } = false;
        public bool NeedValidation { get; set; } = false;

        public UserAddPage()
        {
            InitializeComponent();
        }

        private void LoginAndEnterCheckBox_Checked(
            object sender,
            Microsoft.UI.Xaml.RoutedEventArgs e
        )
        {
            NeedLoginAndEnter = true;
            NeedValidation = true;
        }

        private void ValidationCheckBox_Checked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            NeedValidation = true;
        }

        private void LoginAndEnterCheckBox_Unchecked(
            object sender,
            Microsoft.UI.Xaml.RoutedEventArgs e
        )
        {
            NeedLoginAndEnter = false;
        }

        private void ValidationCheckBox_Unchecked(
            object sender,
            Microsoft.UI.Xaml.RoutedEventArgs e
        )
        {
            NeedValidation = false;
        }
    }
}
