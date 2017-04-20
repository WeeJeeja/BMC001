using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class WeekOverview
    {
        public WeekOverview()
        {
            Resources = new List<ResourceOverview>();
        }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DotNet.Highcharts.Highcharts Chart { get; set; }

        public int Frequency { get; set; }

        public int Occupancy { get; set; }

        public int Utilisation { get; set; }

        public List<ResourceOverview> Resources { get; set; }
    }

}