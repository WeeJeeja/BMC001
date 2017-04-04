using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class UnconfirmedEntry
    {
        public Guid? UnconfirmedBookingId { get; set; }

        public DateTime Date { get; set; }

        public Slot Slot { get; set; }

        public string StartTime { get; set; }

        public Resource Resource { get; set; }

        public User Creator { get; set; }

        [Display(Name = "Booked by")]
        public string BookedBy { get; set; }
    }
}