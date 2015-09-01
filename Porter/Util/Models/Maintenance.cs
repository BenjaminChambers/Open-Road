using System;

namespace Porter.Util.Models
{
    public class Maintenance : BaseItem
    {
        public enum ReminderType { None=0, Date, Mileage, Both }

        // Constructors
        public Maintenance()
        {
            CarID = Settings.CurrentCar;
        }

        // Properties
        public string Description { get; set; }
        public ReminderType Reminder { get; set; }
        public DateTime NextDate { get; set; }
        public int NextMileage { get; set; }

        public string ReminderDescription
        {
            get
            {
                switch (Reminder)
                {
                    case ReminderType.Date:
                        return "Next due on " + Format.Date(NextDate);
                    case ReminderType.Mileage:
                        return "Next due at " + Format.Miles(NextMileage);
                    case ReminderType.Both:
                        return "Next due at " + Format.Miles(NextMileage) + " or on " + Format.Date(NextDate);
                    default:
                        return "";
                }
            }
        }
    }
}
