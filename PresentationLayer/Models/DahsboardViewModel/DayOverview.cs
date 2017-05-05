using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class DayOverview
    {
        public DayOverview()
        {
            Slots = new List<SlotOverview>();
        }

        public DotNet.Highcharts.Highcharts DayChart { get; set; }

        public string Day { get; set; }

        public DateTime Date { get; set; }

        public string Frequency { get; set; }

        public string Occupancy { get; set; }

        public string Utilisation { get; set; }

        public List<SlotOverview> Slots { get; set; }

        public DateInformation DateInformation { get; set; }
    }
}
