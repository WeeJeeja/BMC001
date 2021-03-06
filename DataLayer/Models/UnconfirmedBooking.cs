﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class UnconfirmedBooking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? UnconfirmedBookingId { get; set; }

        public DateTime Date { get; set; }

        public virtual Slot Slot { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual User User { get; set; }

        public virtual User BookedBy { get; set; }
    }
}
