using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.WrapperModels
{
    public class User
    {
        public Guid? UserId { get; set; }

        public int EmployeeNumber { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string JobTitle { get; set; }

        public bool IsLineManager { get; set; }

        public bool IsAdministrator { get; set; }

        public Guid? Team { get; set; }

        public User LineManager { get; set; }

        public string Password { get; set; }

        public DateTime? CancellationDate { get; set; }
    }
}
