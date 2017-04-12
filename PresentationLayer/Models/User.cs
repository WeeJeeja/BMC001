using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Models
{
    public class User
    {
        public Guid? UserId { get; set; }

        [Required]
        [Display(Name = "Employee number")]
        public int EmployeeNumber { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Job title")]
        public string JobTitle { get; set; }

        [Display(Name = "Line manager")]
        public bool IsLineManager { get; set; }

        [Display(Name = "Administrator")]
        public bool IsAdministrator { get; set; }

        public User LineManager { get; set; }
    }
}
