using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Porter.Util.Maintenance
{
    public class MaintenanceForm : NotifyPropertyChangedBase, IForm<MaintenanceForm, Maintenance>
    {
        public MaintenanceForm()
        {
            Date = DateTime.Now;
        }

        public void From(int RecordID)
        {
            using (var db = Database.Connection())
            {
                From(db.Get<Maintenance>(RecordID));
            }
        }
        
        public void From(Maintenance item)
        {
            Description = item.Description;
            Date = item.Date;
            Odometer = item.Odometer;
            Cost = item.Cost;

            Altitude = item.Altitude;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
        }

        public int Insert()
        {
            Maintenance item = new Maintenance();
            Update(item);
            using (var db = Database.Connection())
            {
                db.Insert(item);
            }
            Database.UploadAsync();
            return item.ID;
        }
        public void Update(int RecordID)
        {
            using (var db = Database.Connection())
            {
                var item = db.Get<Maintenance>(RecordID);
                Update(item);
                db.Update(item);
            }
            Database.UploadAsync();
        }
        private void Update(Maintenance Item)
        {
            Item.Description = Description;
            Item.Date = Date.Date;
            Item.Odometer = Odometer;
            Item.Cost = Cost;

            Item.Altitude = Altitude;
            Item.Latitude = Latitude;
            Item.Longitude = Longitude;
        }

        public string Description { get { return _description; } set { SetField(ref _description, value); } }
        public DateTimeOffset Date { get { return _date; } set { SetField(ref _date, value); } }
        public int Odometer { get { return _odometer; } set { SetField(ref _odometer, value); } }
        public double Cost { get { return _cost; } set { SetField(ref _cost, value); } }

        public double Altitude { get { return _altitude; } set { SetField(ref _altitude, value); } }
        public double Latitude { get { return _latitude; } set { SetField(ref _latitude, value); } }
        public double Longitude { get { return _longitude; } set { SetField(ref _longitude, value); } }

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
                OnPropertyChanged("Location");
            }
        }

        private string _description;

        private DateTimeOffset _date;
        private int _odometer;
        private double _cost;

        private double _altitude;
        private double _latitude;
        private double _longitude;
    }
}
