﻿using System.IO;

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
    }
}
