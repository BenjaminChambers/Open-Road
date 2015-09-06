using System;

namespace Porter.Util.Maintenance
{
    public class Maintenance : BaseItem
    {
        // Constructors
        public Maintenance()
        {
            CarID = Settings.CurrentCar;
        }

        // Properties
        public string Description { get; set; }
    }
}
