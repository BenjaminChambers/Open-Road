using System;
using Windows.Devices.Geolocation;

namespace Porter.Util.Models
{
    public abstract class BaseItem
    {
        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int ID { get; set; }

        public DateTime Date { get; set; }
        public int Odometer { get; set; }
        public double Cost { get; set; }

        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int CarID { get; set; }

        public async void SetLocationToCurrent()
        {
            Geoposition pos = await new Geolocator().GetGeopositionAsync();

            Altitude = pos.Coordinate.Point.Position.Altitude;
            Latitude = pos.Coordinate.Point.Position.Latitude;
            Longitude = pos.Coordinate.Point.Position.Longitude;
        }
        
    }
}
