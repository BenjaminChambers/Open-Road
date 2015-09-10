using System;

namespace Porter.Util.Maintenance
{
    public class Maintenance : BaseItem
    {
        // Constructors
        public Maintenance()
        {
            CarID = Settings.CurrentCarID;
        }

        // Properties
        public string Description { get; set; }
    }
}
