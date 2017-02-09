using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainLayer.WrapperModels
{
    public class Team
    {
        public Guid? TeamId { get; set; }

        public string Name { get; set; }

        public string Colour { get; set; }

        public ICollection<User> Members { get; set; }
    }
}
