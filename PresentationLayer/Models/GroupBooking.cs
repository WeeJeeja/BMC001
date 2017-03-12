using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class GroupBooking
    {
        public GroupBooking()
        {
            Occupants = new List<User>();
            Teams     = new List<Team>();
        }
        public DateTime Date { get; set; }

        public Guid? StartTime { get; set; }

        public Guid? EndTime { get; set; }

        public ICollection<User> Occupants { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}