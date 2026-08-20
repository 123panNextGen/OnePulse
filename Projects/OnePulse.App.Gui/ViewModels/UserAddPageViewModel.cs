using CommunityToolkit.Mvvm.ComponentModel;

namespace OnePulse.App.Gui.ViewModels
{
    public partial class UserAddPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial bool NeedLoginAndEnter { get; set; }

        [ObservableProperty]
        public partial bool NeedValidation { get; set; }

        [ObservableProperty]
        public partial string UserName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        partial void OnNeedLoginAndEnterChanged(bool value)
        {
            if (value)
                NeedValidation = true;
        }
    }
}
