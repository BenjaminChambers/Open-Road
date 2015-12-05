using Microsoft.OneDrive.Sdk.WinStore;
using System.IO;
using System;
using System.Threading.Tasks;

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

                if (cars.Count() == 0)
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

        public static string[] OneDriveScopes = { "wl.signin", "onedrive.appfolder", "wl.offline_access" };
        public static async void UploadAsync()
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

        public static async Task DownloadAsync(string fName)
        {
            var OneDriveClient = OneDriveClientExtensions.GetUniversalClient(OneDriveScopes);
            await OneDriveClient.AuthenticateAsync();

            var localFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("porter.sqlite", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            var localStream = await localFile.OpenStreamForWriteAsync();

            var remoteStream = await OneDriveClient.Drive.Special.AppRoot.ItemWithPath(fName).Content.Request().GetAsync();

            await remoteStream.CopyToAsync(localStream);
            await localStream.FlushAsync();

            return;
        }
    }
}
