using PresentationLayer.Models;
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
    public class BlockBooking
    {
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name="Start date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Start time")]
        public Guid? StartSlot { get; set; }

        [Display(Name = "End time")]
        public Guid? EndSlot { get; set; }

        [Required]
        public User User { get; set; }

    }
}
