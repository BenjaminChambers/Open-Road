using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Util.Reminder
{
    public class Reminder
    {
        public enum ReminderType { None = 0, Date, Mileage, Both }

        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int ID { get; set; }

        public string Description { get; set; }
        public ReminderType Type { get; set; }
        public DateTime NextDate { get; set; }
        public int NextMileage { get; set; }

        [SQLite.Ignore]
        public string ReminderDescription
        {
            get
            {
                switch (Type)
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
