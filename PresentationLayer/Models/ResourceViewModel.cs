using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class ResourceViewModel
    {
        public Resource resource { get; set; }

        public int Frequency { get; set; }

        public int Occupancy { get; set; }

        public int Utilisation { get; set; }
    }
}
