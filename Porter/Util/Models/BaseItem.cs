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
            Location = pos.Coordinate.Point;
        }
        
        [SQLite.Ignore]
        public Geopoint Location
        {
            get
            {
                BasicGeoposition pos = new BasicGeoposition();
                pos.Altitude = Altitude;
                pos.Latitude = Latitude;
                pos.Longitude = Longitude;

                return new Geopoint(pos);
            }
            set
            {
                Altitude = value.Position.Altitude;
                Latitude = value.Position.Latitude;
                Longitude = value.Position.Longitude;
            }
        }
    }
}
