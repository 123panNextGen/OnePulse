using OnePulse.Features.LoginManager.Services.Interface;

namespace OnePulse.Features.LoginManager.Services.SecureCrypto
{
    // LiteDB 数据库密码密钥库
    // 密钥由 DPAPI 加密后落盘，与数据库同目录；随用户拷贝数据库而不带密钥即无法解密
    public class SecureKeyStore : ISecureKeyStore
    {
        private readonly string _keyFilePath;
        string? _key;

        public SecureKeyStore(string appDataPath)
        {
            _keyFilePath = appDataPath + @"\Database\key.dat";
        }

        public string Key
        {
            get
            {
                // 懒加载：多次连接数据库只需解密一次
                if (_key == null)
                    LoadKey();
                return _key!;
            }
        }

        // 返回数据库加密密钥，若尚未生成则创建一个新的随机密钥
        private void LoadKey()
        {
            if (File.Exists(_keyFilePath))
            {
                _key = SecureCryptoService.Unprotect(File.ReadAllText(_keyFilePath));
            }
            else
            {
                _key = SecureCryptoService.GenerateRandomKey();
                // 只有当前用户能解密，备份密钥文件无意义（换机不可用）
                File.WriteAllText(_keyFilePath, SecureCryptoService.Protect(_key));
            }
        }
    }
}