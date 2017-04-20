using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class ResourceRateTable
    {
        public Slot Slot { get; set; }

        public ResourceRateData MondayRates { get; set; }

        public ResourceRateData TuesdayRates { get; set; }

        public ResourceRateData WednesdayRates { get; set; }

        public ResourceRateData ThursdayRates { get; set; }

        public ResourceRateData FridayRates { get; set; }
    }
}