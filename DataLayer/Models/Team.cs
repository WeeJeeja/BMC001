using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataLayer.Models
{
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? TeamId { get; set; }

        public string Name { get; set; }

        public string Colour { get; set; }

        public virtual ICollection<User> Members { get; set; }
    }
}
