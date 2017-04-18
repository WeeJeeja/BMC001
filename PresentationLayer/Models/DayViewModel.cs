using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class DayViewModel
    {
        public DotNet.Highcharts.Highcharts DayChart { get; set; }

        public string Day { get; set; }

        public DateTime Date { get; set; }

        public float Frequency { get; set; }

        public float Occupancy { get; set; }

        public float Utilisation { get; set; }
    }
}
