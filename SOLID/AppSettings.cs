using System.Configuration;

namespace SOLID
{
    class AppSettings
    {
        static private int GetIntSetting(string setting)
        {
            return int.Parse(ConfigurationManager.AppSettings[setting]);
        }
        static public int GetMin()
        {
            return GetIntSetting("min");
        }
        static public int GetMax()
        {
            return GetIntSetting("max");
        }
        static public int GetAttempts()
        {
            return GetIntSetting("attempts");
        }
    }
}
