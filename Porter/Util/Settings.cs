using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Porter.Util
{
    public class Settings
    {
        public static int CurrentCarID { get { return GetValue<int>(); } set { SetValue(value); } }
        public static Car.Car CurrentCar {
            get {
                using (var db = Database.Connection())
                {
                    return db.Get<Car.Car>(CurrentCarID);
                }
            }
        }

        public static bool PreferGPM { get { return GetValue<bool>(); } set { SetValue(value); } }

        public static bool ShowFillupRecent { get { return GetValue<bool>(); } set { SetValue(value); } }
        public static bool ShowFillupMonthly { get { return GetValue<bool>(); } set { SetValue(value); } }
        public static bool ShowFillupAnnual { get { return GetValue<bool>(); } set { SetValue(value); } }
        public static bool ShowFillupTotal { get { return GetValue<bool>(); } set { SetValue(value); } }

        public static bool ShowMaintenanceRecent { get { return GetValue<bool>(); } set { SetValue(value); } }
        public static bool ShowMaintenanceAnnual { get { return GetValue<bool>(); } set { SetValue(value); } }
        public static bool ShowMaintenanceTotal { get { return GetValue<bool>(); } set { SetValue(value); } }


        private static T GetValue<T>([CallerMemberName] string name = null)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var result = settings.Values[name];
            if (result == null)
            {
                if (Defaults.ContainsKey(name))
                    settings.Values[name] = Defaults[name];
                else
                    settings.Values[name] = default(T);

                return (T)settings.Values[name];
            }
            return (T)result;
        }

        private static void SetValue<T>(T value, [CallerMemberName] string name = null)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[name] = value;
        }

        private static Dictionary<string, bool> Defaults = new Dictionary<string, bool>
        {
            ["ShowFillupRecent"] = true,
            ["ShowFillupMonthly"] = false,
            ["ShowFillupAnnual"] = true,
            ["ShowFillupTotal"] = false,

            ["ShowMaintenanceRecent"] = false,
            ["ShowMaintenanceAnnual"] = false,
            ["ShowMaintenanceTotal"] = false,
        };
    }
}
