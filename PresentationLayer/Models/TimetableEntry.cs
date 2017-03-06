using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class TimetableEntry
    {
        public string Time { get; set; }

        [Display(Name="Monday")]
        public string MondayResource { get; set; }


        [Display(Name = "Tuesday")]
        public string TuesdayResource { get; set; }


        [Display(Name = "Wednesday")]
        public string WednesdayResource { get; set; }
    }
}