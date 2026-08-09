namespace OnePulse.Features.LoginManager.Services.Interface
{
    // 延迟到服务全部注册后再初始化：
    // 数据库密码来自 SecureKeyStore.Key（首次访问才懒生成），
    // 若在构造函数中直接依赖，会受 LoginManager 属性赋值顺序影响
    public partial interface IUtilityService
    {
        public void Initialize();
    }
}