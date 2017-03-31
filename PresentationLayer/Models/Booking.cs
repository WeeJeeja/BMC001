﻿using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PresentationLayer.Models
{
    public class Booking
    {

        public Booking()
        {
            Resources = new List<Resource>();
        }
        public Guid? BookingId { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        public IEnumerable<Slot> Slots { get; set; }

        public Guid? Slot { get; set; }

        public ICollection<Resource> Resources { get; set; }

        public Guid? Resource { get; set; }

        [Required]
        public User User { get; set; }

        //Index display

        public string ResourceName { get; set; }

        public string Time { get; set; }
    }
}
