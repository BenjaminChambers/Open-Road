using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Util.Models
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
