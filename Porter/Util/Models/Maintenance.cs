using System;

namespace Porter.Util.Models
{
    public class Maintenance : BaseItem
    {
        // Constructors
        public Maintenance()
        {
            CarID = Settings.CurrentCar;
            ReminderID = -1;
        }

        // Properties
        public string Description { get; set; }
        public int ReminderID { get; set; }
    }
}
