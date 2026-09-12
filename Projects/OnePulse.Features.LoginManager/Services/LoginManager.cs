using LiteDB;
using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services.Interface;
using OnePulse.Features.LoginManager.Services.SecureCrypto;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        private static readonly Lazy<LoginManager> Lazy = new(() => new LoginManager());

        public static LoginManager Instance => Lazy.Value;

        // 子服务
        private UtilityService Utils { get; }
        public IAddService Add { get; }
        public IGetService Get { get; }
        public IDeleteService Delete { get; }
        private ISecureKeyStore KeyStore { get; }
        public IUserInfoConverter Converter { get; }

        private readonly string _appDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\OnePulse";

        private LiteDatabase? Database { get; set; }
        private ILiteCollection<StorageUser>? UserCollections { get; set; }

        public LoginManager()
        {
            Directory.CreateDirectory(_appDataPath);
            Directory.CreateDirectory(_appDataPath + @"\Database");

            // 注册服务
            KeyStore = new SecureKeyStore(_appDataPath);
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
