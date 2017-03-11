using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Slot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? SlotId { get; set; }

        public string Time { get; set; }

        public TimeSpan TimeFormat { get; set; }
    }
}
