using System;

namespace Porter.Util.Models
{
    public class Maintenance : BaseItem
    {
        public enum ReminderType { None=0, Date, Mileage, Both }

        // Constructors
        public Maintenance()
        {

        }

        // Properties
        public string Description { get; set; }
        public ReminderType Reminder { get; set; }
        public DateTime NextDate { get; set; }
        public int NextMileage { get; set; }
    }
}
