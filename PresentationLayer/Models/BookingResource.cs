using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class BookingResource
    {
        public Booking Booking { get; set; }
        public Guid ResourceId { get; set; }
    }
}