using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.WrapperModels
{
    public class Booking
    {
        public Booking()
        {
            ConfirmedAttendees   = new List<User>();
            UnconfirmedAttendees = new List<User>();
        }
        public Guid? BookingId { get; set; }

        public DateTime Date { get; set; }

        public Slot Slot { get; set; }

        public Resource Resource { get; set; }

        public User User { get; set; }

        public User BookedBy { get; set; }

        public bool? GroupBooking { get; set; }

        public ICollection<User> ConfirmedAttendees { get; set; }

        public ICollection<User> UnconfirmedAttendees { get; set; }
    }
}
