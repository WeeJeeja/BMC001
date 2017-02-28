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
            MondayEntries = new List<TimetableEntry>();

            TuesdayEntries = new List<TimetableEntry>();

            WednesdayEntries = new List<TimetableEntry>();

            ThursdayEntries = new List<TimetableEntry>();

            FridayEntries = new List<TimetableEntry>();
        }

        public ICollection<Slot> Slots { get; set; }
        public ICollection<TimetableEntry> MondayEntries { get; set; }
        public ICollection<TimetableEntry> TuesdayEntries { get; set; }
        public ICollection<TimetableEntry> WednesdayEntries { get; set; }
        public ICollection<TimetableEntry> ThursdayEntries { get; set; }
        public ICollection<TimetableEntry> FridayEntries { get; set; }
    }
}