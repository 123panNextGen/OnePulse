namespace OnePulse.Pan123.Api.Models.UserInfo
{
    public class VipInfo
    {
        public int? VipLevel { get; set; }
        public string? VipLabel { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public VipInfo() { }
    }

    public class DeveloperInfo
    {
        public DateTime? StartTime;
        public DateTime? EndTime;
    }

    public class OpenUserInfo
    {
        // 用户信息
        public int Uid { get; set; }
        public string? Nickname { get; set; }
        public string? HeadImage { get; set; }
        public string? Passport { get; set; }
        public string? Mail { get; set; }

        // 空间
        public long SpaceUsed { get; set; }
        public long SpacePermanent { get; set; }
        public long SpaceTemp { get; set; }
        public string? SpaceTempExpr { get; set; }

        public bool Vip { get; set; }
        public int? DirectTraffic { get; set; }
        public bool IsHideUID { get; set; }
        public int? HttpsCount { get; set; }

        public List<VipInfo>? VipInfo { get; set; }
        public DeveloperInfo? DeveloperInfo { get; set; }

        public OpenUserInfo() { }
    }
}
