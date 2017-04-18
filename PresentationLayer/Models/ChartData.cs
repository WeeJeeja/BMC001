using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class ChartData
    {
        public string yCategories { get; set; }

        public int Frequency { get; set; }

        public int Occupancy { get; set; }

        public int Utilisation { get; set; }
    }
}