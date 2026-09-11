using System.IO;

namespace OnePulse.Pan123.Api.Data
{
    public static class Constants
    {
        public static class StorageUser
        {
            public const int LatestVersion = 1;
        }

        public static class UserInfo
        {
            public static string DeviceDataPath = Path.Combine("Data", "DeviceData.json");
        }
    }
}