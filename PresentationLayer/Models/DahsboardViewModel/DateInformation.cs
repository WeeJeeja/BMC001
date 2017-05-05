using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class DateInformation
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ActiveDay { get; set; }
    }
}