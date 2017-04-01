using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class User
    {
        public User()
        {
            Teams = new List<Team>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? UserId { get; set; }

        public int EmployeeNumber { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string JobTitle { get; set; }

        public bool IsLineManager { get; set; }

        public bool IsAdministrator { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual User LineManager { get; set; }
    }
}
