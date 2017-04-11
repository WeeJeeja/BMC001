using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class UpdateBooking
    {
        public UpdateBooking()
        {
            PotentialAttendees = new List<User>();
            SelectedAttendees = new List<string>();
        }

        public Booking Booking { get; set; }

        public ICollection<User> PotentialAttendees { get; set; }

        public IList<string> SelectedAttendees { get; set; }
    }
}