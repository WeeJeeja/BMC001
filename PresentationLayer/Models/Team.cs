using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class Team
    {
        public Guid? TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Colour { get; set; }

        public ICollection<User> Members { get; set; }
    }
}
