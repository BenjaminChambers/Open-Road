using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Util.ViewModels
{
    public class ReminderForm : NotificationBase
    {
        public ReminderForm()
        {
            Type = Models.Maintenance.ReminderType.Both;
            MileageInterval = 0;
            NextDate = DateTime.Now;
        }

        public ReminderForm(Util.Models.Maintenance src)
        {
            Type = src.Reminder;
            NextDate = src.NextDate;
            MileageInterval = src.NextMileage;
        }

        public void Update(Util.Models.Maintenance target)
        {
            target.Reminder = Type;
            target.NextDate = NextDate;
            target.NextMileage = MileageInterval;
        }

        private DateTime _nextDate;
        private int _mileageInterval;
        private Models.Maintenance.ReminderType _type;

        public DateTime NextDate { get { return _nextDate; } set { SetField(ref _nextDate, value); } }
        public int MileageInterval { get { return _mileageInterval; } set { SetField(ref _mileageInterval, value); } }
        public Models.Maintenance.ReminderType Type { get { return _type; } set { SetField(ref _type, value); } }
    }
}
