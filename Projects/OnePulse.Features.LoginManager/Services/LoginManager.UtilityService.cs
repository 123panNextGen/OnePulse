using LiteDB;
using OnePulse.Pan123.Api.Models.UserInfo;
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
                // 修复：原实现把 AppDataPath 覆盖为相对路径，导致数据库位置漂移
                var databasePath = _session.AppDataPath + @"\Database\UserInfo.db";
                var connectionString = $"Filename={databasePath};Password={_session.KeyStore.Key}";

                _session.Database = new LiteDatabase(connectionString);
                _session.UserInfoCollections = _session.Database.GetCollection<UserInfo>();
            }
        }
    }
}