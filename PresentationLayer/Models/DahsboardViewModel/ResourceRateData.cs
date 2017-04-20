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
    }
}