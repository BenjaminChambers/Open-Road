using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Util
{
    public abstract class Settings
    {
        public static bool PreferGPM { get { return GetValue("PreferGPM", false); } set { SetValue("PreferGPM", value); } }

        private static T GetValue<T>(string name, T defaultValue)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var result = settings.Values[name];
            if (result == null)
            {
                settings.Values[name] = defaultValue;
                return defaultValue;
            }
            return (T)result;
        }

        private static void SetValue<T>(string name, T value)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[name] = value;
        }
    }
}
