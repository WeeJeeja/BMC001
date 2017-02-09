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
        public Guid? BookingId { get; set; }

        public DateTime Date { get; set; }

        public Slot Slot { get; set; }

        public Resource Resource { get; set; }

        public User User { get; set; }

        public int Capacity { get; set; }
    }
}
