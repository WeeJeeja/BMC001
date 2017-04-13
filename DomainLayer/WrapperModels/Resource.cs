using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.WrapperModels
{
    public class Resource
    {
        public Guid? ResourceId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        [Required]
        public int Capacity { get; set; }

        public string Location { get; set; }

        public DateTime? CancellationDate { get; set; }
    }
}
