using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationLayer.Models
{
    public class ResourceOverview
    {
        public ResourceOverview()
        {
            Table = new List<ResourceRateTable>();
        }

        public List<ResourceRateTable> Table { get; set; }

        public Resource Resource { get; set; }

        public string Frequency { get; set; }

        public string Occupancy { get; set; }

        public string Utilisation { get; set; }

        public DateInformation DateInformation { get; set; }
    }
}
