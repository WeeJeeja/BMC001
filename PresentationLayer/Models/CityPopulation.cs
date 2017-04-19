using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class CityPopulation
    {
        public string city_name { get; set; }
        public int population { get; set; }
        public string id { get; set; }
        public string year { get; set; }
    }
}