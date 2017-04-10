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
            TimetableDisplay     = "---";
            ConfirmedAttendees   = new List<User>();
            UnconfirmedAttendees = new List<User>();
        }

        public Guid? BookingId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name="Time")]
        public Slot Slot { get; set; }

        public Resource Resource { get; set; }

        public User User { get; set; }

        public User BookedBy { get; set; }

        public bool? GroupBooking { get; set; }

        [Display(Name = "Resource")]
        public string TimetableDisplay { get; set; }

        public ICollection<User> ConfirmedAttendees { get; set; }

        public ICollection<User> UnconfirmedAttendees { get; set; }
    }
}
