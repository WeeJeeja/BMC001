using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class ResourceRateData
    {
        public User BookedBy { get; set; }

        public int NumberOfAttendees { get; set; }

        public float Frequency { get; set; }

        public float Occupancy { get; set; }

        public float Utilisation { get; set; }
    }
}