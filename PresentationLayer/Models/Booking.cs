using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PresentationLayer.Models
{
    public class Booking
    {
        public Booking()
        {
            TimetableDisplay = "---";
        }

        public Guid? BookingId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        public Slot Slot { get; set; }

        public Resource Resource { get; set; }

        public User User { get; set; }

        public User BookedBy { get; set; }

        public string TimetableDisplay { get; set; }
    }
}
