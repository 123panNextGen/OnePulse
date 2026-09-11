using CommunityToolkit.Mvvm.ComponentModel;
using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;
using System;
using System.Threading.Tasks;

namespace OnePulse.App.Gui.ViewModels
{
    public partial class UserAddPageViewModel : ObservableObject
    {
        UserInfoViewModel UserInfoVM { get; set; } = new();

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

        private UserInfo GetUserInfo()
        {
            return new UserInfo()
            {
                UserName = UserName,
                Password = Password,
                Uuid = Guid.NewGuid().ToString(),
                DeviceInfo = DeviceInfo.DeviceInfoFromString("Xiaomi:17"),
            };
        }

        public async Task<ApiReturn<UserInfo>> LoginAsync()
        {
            UserInfo result = await UserInfoVM.LoginAsync(GetUserInfo(), true);

            return new ApiReturn<UserInfo>(ApiResult.Success) { Data = result };
        }
    }
}
