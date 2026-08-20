using CommunityToolkit.Mvvm.ComponentModel;

namespace OnePulse.App.Gui.ViewModels
{
    public partial class UserAddPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial bool NeedLoginAndEnter { get; set; }

        [ObservableProperty]
        public partial bool NeedValidation { get; set; }

        partial void OnNeedLoginAndEnterChanged(bool value)
        {
            if (value)
                NeedValidation = true;
        }
    }
}
