﻿using System;
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

        public DotNet.Highcharts.Highcharts Chart { get; set; }

        public string Frequency { get; set; }

        public string Occupancy { get; set; }

        public string Utilisation { get; set; }

        public List<ResourceOverview> Resources { get; set; }

        public DateInformation DateInformation { get; set; }
    }

}