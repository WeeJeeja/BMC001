using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class TimetableEntry
    {
        public Booking Booking { get; set; }
        public string Time { get; set; }
        public string ResourceName { get; set; }
    }
}