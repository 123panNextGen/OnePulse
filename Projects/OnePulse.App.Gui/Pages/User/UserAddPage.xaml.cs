using Microsoft.UI.Xaml.Controls;
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

        private void CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
