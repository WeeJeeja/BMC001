using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class SlotOverview
    {
        public DotNet.Highcharts.Highcharts FrequencyChart { get; set; }

        public DotNet.Highcharts.Highcharts OccupancyChart { get; set; }

        public string Time { get; set; }

        public DateTime Date { get; set; }

        public float Frequency { get; set; }

        public float Occupancy { get; set; }

        public float Utilisation { get; set; }

        public int NumberOfResources { get; set; }

        public int NumberOfResourcesUsed { get; set; }
    }
}
