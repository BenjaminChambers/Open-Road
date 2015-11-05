using Microsoft.OneDrive.Sdk.WinStore;
using System.IO;
using System;

namespace Porter.Util
{
    public abstract class Database
    {
        private static bool Initialized = false;
        private static string DBPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "porter.sqlite");
        public static SQLite.SQLiteConnection Connection()
        {
            var conn = new SQLite.SQLiteConnection(DBPath);
            if (!Initialized)
            {
                conn.CreateTable<Car.Car>();
                conn.CreateTable<Fillup.Fillup>();
                conn.CreateTable<Maintenance.Maintenance>();
                conn.CreateTable<Reminder.Reminder>();

                var cars = conn.Table<Car.Car>();

                if (cars.Count()==0)
                {
                    Car.Car car = new Car.Car();
                    conn.Insert(car);
                }

                var firstCar = cars.First();

                if (cars.Where(vehicle => vehicle.ID == Settings.CurrentCarID).Count() == 0)
                    Settings.CurrentCarID = firstCar.ID;

                
                

                Initialized = true;
            }
            return conn;
        }
        
        private static string[] OneDriveScopes = { "wl.signin", "onedrive.appfolder", "wl.offline_access" };
        public static async void Upload()
        {
            if (Settings.SaveToOneDrive)
            {
                var OneDriveClient = OneDriveClientExtensions.GetUniversalClient(OneDriveScopes);
                await OneDriveClient.AuthenticateAsync();

                Stream localFile = await (
                    await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("porter.sqlite")
                    ).OpenStreamForReadAsync();

                string fName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".OpenRoad";

                await OneDriveClient.Drive.Special.AppRoot.ItemWithPath(fName).Content.Request().PutAsync<Microsoft.OneDrive.Sdk.Item>(localFile);
            }
        }
        public static async void Download()
        {
            if (Settings.SaveToOneDrive)
            {
                var OneDriveClient = OneDriveClientExtensions.GetUniversalClient(OneDriveScopes);
                await OneDriveClient.AuthenticateAsync();

                var backups = await OneDriveClient.Drive.Special.AppRoot.Children.Request().GetAsync();


            }
        }
    }
}
