using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.WrapperModels
{
    public class Slot
    {
        public Guid? SlotId { get; set; }

        public string Time { get; set; }
    }
}
