﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class ChartViewModel
    {
        public ChartViewModel()
        {
            Resources = new List<ResourceViewModel>();
        }

        public DotNet.Highcharts.Highcharts Chart { get; set; }

        public int Frequency { get; set; }

        public int Occupancy { get; set; }

        public int Utilisation { get; set; }

        public ICollection<ResourceViewModel> Resources { get; set; }
    }
}