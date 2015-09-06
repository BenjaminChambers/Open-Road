using System.IO;

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

                if (conn.Table<Car.Car>().Count()==0)
                {
                    Car.Car car = new Car.Car();
                    conn.Insert(car);
                }

                Initialized = true;
            }
            return conn;
        }
    }
}
