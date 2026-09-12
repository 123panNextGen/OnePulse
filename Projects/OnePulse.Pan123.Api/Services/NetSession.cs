using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Pan123.Api.Services
{
    public partial class NetSession
    {
        private static readonly Lazy<NetSession> Lazy = new(() => new NetSession());

        public static NetSession Instance => Lazy.Value;

        // 子服务
        public AuthService Auth { get; }
        public UtilityService Utils { get; }

        // Http 客户端
        private static readonly HttpClient SharedClient = new()
        {
            BaseAddress = new Uri("https://www.123pan.cn"),
        };
        internal static readonly HttpClient FreeClient = new();

        // 用户信息
        public UserInfo? UserInfo { get; private set; }

        private NetSession()
        {
            // 注册服务
            Auth = new AuthService(this);
            Utils = new UtilityService(this);
        }
    }
}
