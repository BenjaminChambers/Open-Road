using System;
using Windows.Devices.Geolocation;

namespace Porter.Util.Fillup
{
    public class FillupForm : NotifyPropertyChangedBase, IForm<FillupForm, Fillup>
    {
        public FillupForm()
        {
            Date = DateTime.Now;
        }

        public void From(int RecordID)
        {
            using (var db = Database.Connection())
            {
                From(db.Get<Fillup>(RecordID));
            }
        }

        public void From(Fillup item)
        {
            Volume = item.Volume;
            Date = item.Date;
            Odometer = item.Odometer;
            Cost = item.Cost;
            Altitude = item.Altitude;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
        }

        public void Update(int RecordID)
        {
            using (var db = Database.Connection())
            {
                var item = db.Get<Fillup>(RecordID);
                Update(item);
                db.Update(item);
            }
        }
        private void Update(Fillup item)
        {
            item.Volume = Volume;
            item.Date = Date.Date;
            item.Odometer = Odometer;
            item.Cost = Cost;
            item.Altitude = Altitude;
            item.Latitude = Latitude;
            item.Longitude = Longitude;
        }

        public int Insert()
        {
            using (var db = Database.Connection())
            {
                Fillup item = new Fillup();
                Update(item);
                item.SetLocationToCurrent();

                var car = db.Get<Car.Car>(Settings.CurrentCarID);
                item.Cost += car.PartialCost;
                item.Volume += car.PartialVolume;
                car.PartialCost = car.PartialVolume = 0;
                db.Insert(item);

                db.Update(car);

                return item.ID;
            }
        }

        public void AddPartial()
        {
            using (var db = Database.Connection())
            {
                var car = db.Get<Car.Car>(Settings.CurrentCarID);

                car.PartialCost += Cost;
                car.PartialVolume += Volume;
                db.Update(car);

                Volume = 0;
                Cost = 0;
            }
        }

        public double Volume { get { return _volume; } set { SetField(ref _volume, value); } }
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


        private double _volume;

        private DateTimeOffset _date;
        private int _odometer;
        private double _cost;

        private double _altitude;
        private double _latitude;
        private double _longitude;
    }
}
