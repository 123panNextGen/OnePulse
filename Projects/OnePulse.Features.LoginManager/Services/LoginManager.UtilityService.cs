using LiteDB;
using OnePulse.Features.LoginManager.Models;
using OnePulse.Features.LoginManager.Services.Interface;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        public class UtilityService : IUtilityService
        {
            private readonly LoginManager _session;

            internal UtilityService(LoginManager session)
            {
                _session = session;
            }

            // 数据库密码依赖 SecureKeyStore.Key（首次访问才懒生成），
            // 故初始化推迟到 LoginManager 全部子服务注册完成后执行
            public void Initialize()
            {
                string databasePath = _session._appDataPath + @"\Database\UserInfo.db";
                string connectionString = $"Filename={databasePath};Password={_session.KeyStore.Key}";

                _session.Database = new LiteDatabase(connectionString);
                _session.UserCollections = _session.Database.GetCollection<StorageUser>();
            }
        }
    }
}
