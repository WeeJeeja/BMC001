using DataLayer;
using DomainLayer.WrapperModels;
using HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class BookingService : IBookingService
    {

        ModelConversitions converter = new ModelConversitions();
        ISlotService slotService = new SlotService();
        IUserService userService = new UserService();
        IResourceService resourceService = new ResourceService();

        /// <summary>
        /// Gets a list of all bookings for a particular user and week
        /// </summary>
        /// <returns>Returns a list of all of a user's bookings for a particular week</returns>
        public List<Booking> GetThisWeeksBookings(Guid? userId)
        {
            //work out the start of the week
            var date = DateTime.Today;

            switch(date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    {
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        date = date.AddDays(-1);
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        date = date.AddDays(-2);
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        date = date.AddDays(-3);
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        date = date.AddDays(-4);
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        date = date.AddDays(-5);
                        break;
                    }
                case DayOfWeek.Sunday:
                    {
                        date = date.AddDays(-6);
                        break;
                    }
            }

            var endDate = date.AddDays(7);

            //db connection
            var db = new ReScrumEntities();

            //get this weeks bookings
            var data = db.Booking.Where(b => b.Date >= date && b.Date < endDate).ToList();

            var bookings = new List<Booking>();

            if (data.Count < 1) return bookings;

            //the bookings that belong to the user
            data = data.Where(u => u.User.UserId == userId).ToList();

            foreach(DataLayer.Models.Booking b in data)
            {
                var booking = new Booking
                {
                    BookingId      = b.BookingId,
                    Date           = b.Date,
                    Slot           = converter.ConvertDataSlotToWrapper(b.Slot),
                    Resource       = converter.ConvertDataResourceToWrapper(b.Resource),
                    User           = converter.ConvertDataUserToWrapper(b.User),
                    GroupBooking   = b.GroupBooking,
                    AcceptedByUser = b.AcceptedByUser
                };
                bookings.Add(booking);
            }
            return bookings;
        }

        /// <summary>
        /// Gets a list of all bookings for a particular user and week
        /// </summary>
        /// <returns>Returns a list of all of a user's bookings for a particular week</returns>
        public List<Booking> GetThisWeeksUnconfirmedBookings(Guid? userId)
        {
            var bookings = GetThisWeeksBookings(userId);
            var unconfirmedBooking = bookings.Where(b => b.AcceptedByUser == false).ToList();
            return bookings;
        }

        public List<Resource> GetAvailableResources(DateTime date, Guid? time)
        {
            var db = new ReScrumEntities();

            var unavailableResources  = db.Booking.Where(b =>
                    b.Date            == date &&
                    b.Slot.SlotId     == time).Select(r => r.Resource).ToList();

            var availableResources = db.Resources.ToList().Except(unavailableResources).ToList();

            var resources = converter.ConvertDataResourceListToWrapper(availableResources);

            return resources.ToList(); ;
        }

        public List<Resource> GetAvailableResourcesForBlockBooking(DateTime startDate, DateTime endDate, Guid? startSlot, Guid? endSlot)
        {
            var db = new ReScrumEntities();

            var slotService = new SlotService();
            var startTime = slotService.GetSlot(startSlot);
            var endTime = slotService.GetSlot(endSlot);

            var unavailableResources = db.Booking.Where(b =>
                    b.Date >= startDate &&
                    b.Date <= endDate &&
                    b.Slot.StartTime >= startTime.StartTime &&
                    b.Slot.EndTime <= endTime.EndTime).Select(r => r.Resource).ToList();

            var availableResources = db.Resources.ToList().Except(unavailableResources).ToList();

            var resources = converter.ConvertDataResourceListToWrapper(availableResources);

            return resources.ToList(); ;
        }

        public List<Resource> GetAvailableResourcesForGroupBooking(DateTime date, Guid? startSlot, Guid? endSlot, int capacity)
        {
            var db = new ReScrumEntities();

            var slotService = new SlotService();
            var startTime = slotService.GetSlot(startSlot);
            var endTime = slotService.GetSlot(endSlot);

            var unavailableResources = db.Booking.Where(b =>
                    b.Date == date &&
                    b.Slot.StartTime >= startTime.StartTime &&
                    b.Slot.EndTime <= endTime.EndTime).Select(r => r.Resource).ToList();

            var availableResources = db.Resources.ToList().Except(unavailableResources).ToList();

            availableResources.RemoveAll(r => r.Capacity < capacity);

            var resources = converter.ConvertDataResourceListToWrapper(availableResources);

            return resources.ToList(); ;
        }

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        public void AddBooking(Booking booking)
        {
            var db = new ReScrumEntities();

            var slot = db.Slots.Where( s => s.SlotId == booking.Slot.SlotId).FirstOrDefault();
            var user = db.Users.Where(u => u.UserId == booking.User.UserId).FirstOrDefault();
            var resource = db.Resources.Where(r => r.ResourceId == booking.Resource.ResourceId).FirstOrDefault();

            var newBooking = db.Booking.Where(b => b.User.UserId == user.UserId &&
                                                b.Slot.SlotId == slot.SlotId &&
                                                b.Date == booking.Date).FirstOrDefault();
            if (newBooking == null) newBooking = new DataLayer.Models.Booking();

            newBooking.Date     = booking.Date;
            newBooking.Slot     = slot;
            newBooking.Resource = resource;
            newBooking.User     = user;

            if (newBooking.BookingId == null)  db.Booking.Add(newBooking);

            db.SaveChanges();
        }

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        public void AddBlockBooking(DateTime startDate, DateTime endDate, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId)
        {
            var db = new ReScrumEntities();

            var startSlot = db.Slots.Where(s => s.SlotId == startTime).FirstOrDefault();
            var endSlot = db.Slots.Where(s => s.SlotId == endTime).FirstOrDefault();
            var user = db.Users.Where(u => u.UserId == userId).FirstOrDefault();
            var resource = db.Resources.Where(r => r.ResourceId == resourceId).FirstOrDefault();

            var slotList = db.Slots.Where(s => s.StartTime >= startSlot.StartTime &&
                                                s.EndTime <= endSlot.EndTime).ToList();

            // If run on October 20, 2006, the example produces the following output:
            //    CompareTo method returns 1: 10/20/2006 is later than 10/20/2005
            //    CompareTo method returns -1: 10/20/2006 is earlier than 10/20/2007
            var date = startDate;
            while (date <= endDate)
            {
                foreach (DataLayer.Models.Slot slot in slotList)
                {
                    var booking = db.Booking.Where(b => b.User.UserId == user.UserId &&
                                                b.Slot.SlotId == slot.SlotId &&
                                                b.Date == date).FirstOrDefault();
                    if (booking == null) booking = new DataLayer.Models.Booking();
                    
                    booking.Date     = date;
                    booking.Slot     = slot;
                    booking.Resource = resource;
                    booking.User     = user;

                    if (booking.BookingId == null) db.Booking.Add(booking);
                }
                date = date.AddDays(1);
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Adds a new group booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        public void AddGroupBooking(DateTime date, List<string> users, List<string> teams,  Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId)
        {
            var db = new ReScrumEntities();

            var startSlot = db.Slots.Where(s => s.SlotId == startTime).FirstOrDefault();
            var endSlot   = db.Slots.Where(s => s.SlotId == endTime).FirstOrDefault();
            var user      = db.Users.Where(u => u.UserId == userId).FirstOrDefault();
            var resource  = db.Resources.Where(r => r.ResourceId == resourceId).FirstOrDefault();

            var slotList = db.Slots.Where(s => s.StartTime >= startSlot.StartTime &&
                                                s.EndTime <= endSlot.EndTime).ToList();

            //Add booking for user (the person who created the boooking)
            AddBooking(db, slotList, user, resource, date);

            //Add booking for Attendees
            foreach (string data in users)
            {
                var attendee = db.Users.Where(u => u.UserId == new Guid(data)).FirstOrDefault();
                AddBooking(db, slotList, attendee, resource, date);
            }

            //Add booking for team memebers
            foreach (string data in teams)
            {
                var team = db.Teams.Where(u => u.TeamId == new Guid(data)).FirstOrDefault();
                foreach (DataLayer.Models.User member in team.Members)
                {
                    AddBooking(db, slotList, member, resource, date);
                }
            }
                
            db.SaveChanges();
        }

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        public void AddBooking(ReScrumEntities db, List<DataLayer.Models.Slot> slots, DataLayer.Models.User user, DataLayer.Models.Resource resource, DateTime date)
        {
            foreach (DataLayer.Models.Slot slot in slots)
            {
                var booking = db.Booking.Where(b => b.User.UserId == user.UserId &&
                                            b.Slot.SlotId == slot.SlotId &&
                                            b.Date == date).FirstOrDefault();
                if (booking == null) booking = new DataLayer.Models.Booking();

                booking.Date           = date;
                booking.Slot           = slot;
                booking.Resource       = resource;
                booking.User           = user;
                booking.GroupBooking   = true;
                booking.AcceptedByUser = false;

                if (booking.BookingId == null) db.Booking.Add(booking);
            }

            db.SaveChanges();
        }
    }
}
