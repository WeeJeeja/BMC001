using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class Team
    {
        public Team()
        {
            Members = new List<User>();
        }

        public Guid? TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Colour { get; set; }

        public ICollection<User> Members { get; set; }

        public int Count { 
            get
            {
                return this.Count;
            }
            set
            {
                Count = Members.Count();
            }
        }

        public bool Checked { get; set; }
    }
}
