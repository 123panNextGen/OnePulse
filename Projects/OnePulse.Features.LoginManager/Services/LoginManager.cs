using LiteDB;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        private static readonly Lazy<LoginManager> lazy = new(() => new());

        public static LoginManager Instance
        {
            get { return lazy.Value; }
        }

        // 子服务
        internal UtilityService Utils { get; }

        public string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\OnePulse";
        internal LiteDatabase? Database { get; set; }
        ILiteCollection<UserInfo>? UserInfoCollections;

        public LoginManager()
        {
            Directory.CreateDirectory(AppDataPath);
            Directory.CreateDirectory(AppDataPath + @"\Database");

            // 注册服务
            Utils = new(this);
        }
    }
}
