namespace OnePulse.Pan123.Api.Models.UserInfo
{
    public abstract class VipInfo
    {
        public int? VipLevel { get; set; }
        public string? VipLabel { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public abstract class DeveloperInfo
    {
        public DateTime? StartTime;
        public DateTime? EndTime;
    }

    public class OpenUserInfo
    {
        // 用户信息
        public int Uid { get; init; }
        public string? Nickname { get; init; }
        public string? HeadImage { get; init; }
        public string? Passport { get; init; }
        public string? Mail { get; init; }

        // 空间
        public long SpaceUsed { get; init; }
        public long SpacePermanent { get; init; }
        public long SpaceTemp { get; init; }
        public string? SpaceTempExpr { get; init; }

        public bool Vip { get; init; }
        public int? DirectTraffic { get; init; }
        public bool IsHideUid { get; init; }
        public int? HttpsCount { get; init; }

        public List<VipInfo>? VipInfo { get; init; }
        public DeveloperInfo? DeveloperInfo { get; init; }
    }
}
