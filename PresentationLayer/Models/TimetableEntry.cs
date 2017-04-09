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
        public Booking MondayResource { get; set; }
        
        [Display(Name = "Tuesday")]
        public Booking TuesdayResource { get; set; }

        [Display(Name = "Wednesday")]
        public Booking WednesdayResource { get; set; }

        [Display(Name = "Thursday")]
        public Booking ThursdayResource { get; set; }

        [Display(Name = "Friday")]
        public Booking FridayResource { get; set; }
    }
}