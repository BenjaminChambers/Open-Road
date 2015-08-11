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
                conn.CreateTable<Util.Models.Car>();
                conn.CreateTable<Util.Models.Fillup>();
                conn.CreateTable<Util.Models.Maintenance>();

                if (conn.Table<Models.Car>().Count()==0)
                {
                    Models.Car car = new Models.Car();
                    conn.Insert(car);
                }

                Initialized = true;
            }
            return conn;
        }
    }
}
