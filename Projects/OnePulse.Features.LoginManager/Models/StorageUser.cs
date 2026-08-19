using System;
using System.Collections.Generic;
using System.Text;

namespace OnePulse.Features.LoginManager.Models
{
    internal class StorageUser<T>(string userId, string? userName, string? headImageUrl, T userInfo)
    {
        public string UserId { get; set; } = userId;

        public string? UserName { get; set; } = userName;
        public string? HeadImageUrl { get; set; } = headImageUrl;

        public T UserInfo { get; set; } = userInfo;
    }
}
