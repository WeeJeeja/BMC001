using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Office
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? OfficeId { get; set; }

        public string Name { get; set; }
    }
}
