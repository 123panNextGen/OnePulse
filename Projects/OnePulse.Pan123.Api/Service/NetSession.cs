namespace OnePulse.Pan123.Api.Service
{
    public partial class NetSession
    {
        private static readonly Lazy<NetSession> lazy =
           new Lazy<NetSession>(() => new());

        public static NetSession Instance { get { return lazy.Value; } }

        // 子服务
        public AuthService Auth { get; }
        internal UtilityService Utils { get; }

        // Http 客户端
        internal static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://www.123pan.cn"),
        };
        internal static HttpClient freeClient = new();

        // 用户信息
        Model.UserInfo? UserInfo { get; set; }

        private NetSession()
        {
            // 注册服务
            Auth = new(this);
            Utils = new(this);
        }
    }
}
