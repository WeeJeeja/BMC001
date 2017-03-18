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
        public int EmployeeNumber { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public bool IsLineManager { get; set; }

        public bool IsAdministrator { get; set; }

        public User LineManager { get; set; }
    }
}
