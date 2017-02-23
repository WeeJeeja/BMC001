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
        public Guid? BookingId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public IEnumerable<SelectListItem> Slots { get; set; }

        [Required]
        public Slot Slot { get; set; }

        public ICollection<Resource> Resources { get; set; }

        [Required]
        public Resource Resource { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public int Capacity { get; set; }
    }
}
