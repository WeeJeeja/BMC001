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
            Attendees         = new List<User>();
            Teams             = new List<Team>();
            SelectedAttendees = new List<string>();
            SelectedTeams     = new List<string>();
        }
        public DateTime Date { get; set; }

        public Guid? StartTime { get; set; }

        public Guid? EndTime { get; set; }

        public ICollection<User> Attendees { get; set; }

        public IList<string> SelectedAttendees { get; set; }

        public ICollection<Team> Teams { get; set; }

        public IList<string> SelectedTeams { get; set; }

        public int Capacity { get; set; }
    }
}