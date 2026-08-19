using LiteDB;
using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Features.LoginManager.Services.SecureCrypto;
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
        internal IUtilityService Utils { get; }
        public IAddService Add { get; }
        public IGetService Get { get; }
        public IDeleteService Delete { get; }
        public ISecureKeyStore KeyStore { get; }
        public IUserInfoConverter Converter { get; }

        public string AppDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\OnePulse";
        internal LiteDatabase? Database { get; set; }
        internal ILiteCollection<StorageUser>? UserCollections { get; set; }

        public LoginManager()
        {
            Directory.CreateDirectory(AppDataPath);
            Directory.CreateDirectory(AppDataPath + @"\Database");

            // 注册服务
            KeyStore = new SecureKeyStore(AppDataPath);
            Converter = new UserInfoConverter();
            Utils = new UtilityService(this);
            Add = new AddService(this);
            Get = new GetService(this);
            Delete = new DeleteService(this);

            // 数据库初始化依赖 KeyStore.Key，必须在全部服务注册后执行
            Utils.Initialize();
        }
    }
}
