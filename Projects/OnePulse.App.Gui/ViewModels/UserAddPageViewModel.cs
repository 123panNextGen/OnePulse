using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using OnePulse.Pan123.Api.Models;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.App.Gui.ViewModels
{
    public partial class UserAddPageViewModel : ObservableObject
    {
        private UserInfoViewModel UserInfoVm { get; } = new();

        [ObservableProperty]
        public partial bool NeedLoginAndEnter { get; set; }

        [ObservableProperty]
        public partial bool NeedValidation { get; set; }

        [ObservableProperty]
        public partial bool NeedGetOpenInfo { get; set; }

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
            };
        }

        public async Task<ApiReturn<UserInfo>> LoginAsync()
        {
            if (NeedValidation)
            {
                UserInfo? result = await UserInfoVm.LoginAsync(GetUserInfo(), true);
                if (result == null)
                    return new ApiReturn<UserInfo>(ApiResult.Failed, "Login failed.");
                
                
            }
            
            

            return new ApiReturn<UserInfo>(ApiResult.Success);
        }
    }
}
