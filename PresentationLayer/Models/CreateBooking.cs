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
    public class CreateBooking
    {

        public CreateBooking()
        {
            Resources = new List<Resource>();
        }
        
        public IEnumerable<Slot> Slots { get; set; }

        public ICollection<Resource> Resources { get; set; }

        public Guid? Resource { get; set; }

        [Required]
        public User User { get; set; }

        public SingleBooking SingleBooking { get; set; }

        public BlockBooking BlockBooking { get; set; }

        public GroupBooking GroupBooking { get; set; }

    }
}
