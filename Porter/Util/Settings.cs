using System.Runtime.CompilerServices;

namespace Porter.Util
{
    public class Settings
    {
        public static bool PreferGPM { get { return GetSetting<bool>(); } set { SetValue(value); } }

        public static bool ShowFillupRecent { get { return GetSetting<bool>(); } set { SetValue(value); } }
        public static bool ShowFillupMonthly { get { return GetSetting<bool>(); } set { SetValue(value); } }
        public static bool ShowFillupAnnual { get { return GetSetting<bool>(); } set { SetValue(value); } }
        public static bool ShowFillupTotal { get { return GetSetting<bool>(); } set { SetValue(value); } }

        public static T GetSetting<T>([CallerMemberName] string settingName = null)
        {
            return GetValue<T>(settingName);
        }

        private static T GetValue<T>(string name)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var result = settings.Values[name];
            if (result == null)
            {
                settings.Values[name] = default(T);
                return default(T);
            }
            return (T)result;
        }

        private static void SetValue<T>(T value, [CallerMemberName] string name = null)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[name] = value;
        }
    }
}
