using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Util
{
    public class Settings
    {
        public static bool PreferGPM { get { return GetSetting<bool>(); } set { SetValue(value); } }

        public static bool HideFillupRecent { get { return GetSetting<bool>(); } set { SetValue(value); } }
        public static bool HideFillupMonthly { get { return GetSetting<bool>(); } set { SetValue(value); } }
        public static bool HideFillupAnnual { get { return GetSetting<bool>(); } set { SetValue(value); } }
        public static bool HideFillupTotal { get { return GetSetting<bool>(); } set { SetValue(value); } }

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
