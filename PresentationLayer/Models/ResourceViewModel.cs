using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class ResourceViewModel
    {
        public Resource Resource { get; set; }

        public float Frequency { get; set; }

        public float Occupancy { get; set; }

        public float Utilisation { get; set; }
    }
}
