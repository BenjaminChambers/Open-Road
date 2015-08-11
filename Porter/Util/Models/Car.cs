﻿namespace Porter.Util.Models
{
    public class Car : BaseItem
    {
        public Car() { Name = "My Car"; }

        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public int Year { get; set; }
    }
}
