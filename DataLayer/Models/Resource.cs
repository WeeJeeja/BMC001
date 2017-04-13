using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Resource
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? ResourceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; }

        public DateTime? CancellationDate { get; set; }
    }
}
