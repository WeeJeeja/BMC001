using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class ResourceUntilisationViewModel
    {
        public Slot slot { get; set; }

        public User BookedBy { get; set; }

        public int NumberOfAttendees { get; set; }

        public int Frequency { get; set; }

        public int Occupancy { get; set; }

        public int Utilisation { get; set; }
    }
}