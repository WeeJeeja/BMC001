using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationLayer.Models
{
    public class Timetable
    {

        public Timetable()
        {
            TimetableEntries   = new List<TimetableEntry>();
            UnconfirmedEntries = new List<UnconfirmedEntry>();
        }

        public List<TimetableEntry> TimetableEntries { get; set; }

        public List<UnconfirmedEntry> UnconfirmedEntries { get; set; }
    }
}