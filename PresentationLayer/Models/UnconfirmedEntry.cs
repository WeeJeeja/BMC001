using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class UnconfirmedEntry
    {

        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Resource { get; set; }

        [Display(Name = "Booked by")]
        public string BookedBy { get; set; }
    }
}