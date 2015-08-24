using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Porter.Util.ViewModels
{
    public class MaintenanceForm : NotificationBase
    {
        public MaintenanceForm()
        {
            Date = DateTime.Now;
        }

        public MaintenanceForm(Models.Maintenance item)
        {
            Description = item.Description;
            Date = item.Date;
            Odometer = item.Odometer;
            Cost = item.Cost;
            Altitude = item.Altitude;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
        }

        public void Update(Models.Maintenance item)
        {
            item.Description = Description;
            item.Date = Date.Date;
            item.Odometer = Odometer;
            item.Cost = Cost;
            item.Altitude = Altitude;
            item.Latitude = Latitude;
            item.Longitude = Longitude;
        }
        public Models.Maintenance ToMaintenance()
        {
            Models.Maintenance item = new Models.Maintenance();
            item.SetLocationToCurrent();

            Update(item);
            return item;
        }

        public string Description { get { return _description; } set { SetField(ref _description, value); } }
        public DateTimeOffset Date { get { return _date; } set { SetField(ref _date, value); } }
        public int Odometer { get { return _odometer; } set { SetField(ref _odometer, value); } }
        public double Cost { get { return _cost; } set { SetField(ref _cost, value); } }

        public double Altitude { get { return _altitude; } set { SetField(ref _altitude, value); } }
        public double Latitude { get { return _latitude; } set { SetField(ref _latitude, value); } }
        public double Longitude { get { return _longitude; } set { SetField(ref _longitude, value); } }

        public Models.Maintenance.ReminderType ReminderType { get { return _reminderType; } set { SetField(ref _reminderType, value); } }
        public int ReminderMileage { get { return _reminderMileage; } set { SetField(ref _reminderMileage, value); } }
        public DateTime ReminderDate { get { return ReminderDate; } set { SetField(ref _reminderDate, value); } }

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
        private string _reminder;

        private DateTimeOffset _date;
        private int _odometer;
        private double _cost;

        private double _altitude;
        private double _latitude;
        private double _longitude;

        private Models.Maintenance.ReminderType _reminderType;
        private int _reminderMileage;
        private DateTime _reminderDate;
    }
}
