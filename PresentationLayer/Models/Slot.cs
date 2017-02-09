using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Models
{
    public class Slot
    {
        public Guid? SlotId { get; set; }

        [Required]
        public string Time { get; set; }
    }
}
