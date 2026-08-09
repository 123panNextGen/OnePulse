using LiteDB;
using OnePulse.Pan123.Api.Models.UserInfo;

namespace OnePulse.Features.LoginManager.Services
{
    public partial class LoginManager
    {
        public class UtilityService
        {
            private readonly LoginManager _session;

            readonly string Password = "OnePulse";
            readonly string DataBasePath;
            readonly string connectionString;

            internal UtilityService(LoginManager session)
            {
                _session = session;

                // 初始化数据库
                DataBasePath = _session.AppDataPath = @"\Database\UserInfo.db";
                connectionString = $"Filename={DataBasePath};Password={Password}";

                _session.Database = new LiteDatabase(connectionString);

                _session.UserInfoCollections = _session.Database.GetCollection<UserInfo>();
            }
        }
    }
}
