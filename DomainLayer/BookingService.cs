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
            var date = FindStartDate();

            var endDate = date.AddDays(7);

            //db connection
            var db = new ReScrumEntities();

            //the bookings that belong to the user
            var data = db.Booking.Where(u => u.User.UserId == userId).ToList();

            var bookings = new List<Booking>();
            if (data.Count < 1) return bookings;

            //get this weeks bookings
            data = data.Where(b => b.Date >= date && b.Date < endDate).ToList();

            foreach(DataLayer.Models.Booking b in data)
            {
                var booking = new Booking
                {
                    BookingId      = b.BookingId,
                    Date           = b.Date,
                    Slot           = converter.ConvertDataSlotToWrapper(b.Slot),
                    Resource       = converter.ConvertDataResourceToWrapper(b.Resource),
                    User           = converter.ConvertDataUserToWrapper(b.User),
                    BookedBy       = converter.ConvertDataUserToWrapper(b.BookedBy),
                };
                bookings.Add(booking);
            }
            return bookings;
        }

        /// <summary>
        /// Gets a list of all unconfirmed bookings for a particular user and week
        /// </summary>
        /// <returns>Returns a list of all of a user's unconfirmed bookings for a particular week</returns>
        public List<Booking> GetThisWeeksUnconfirmedBookings(Guid? userId)
        {
            //work out the start of the week
            var date = FindStartDate();

            var endDate = date.AddDays(7);

            //db connection
            var db = new ReScrumEntities();

            //the bookings that belong to the user
            var data = db.UnconfirmedBooking.Where(u => u.User.UserId == userId).ToList();

            var bookings = new List<Booking>();
            if (data.Count < 1) return bookings;

            var test = new DateTime(date.Year, date.Month, date.Day);

            //get this weeks bookings
            data = data.Where(b => b.Date >= test && b.Date < endDate).ToList();

            foreach (DataLayer.Models.UnconfirmedBooking b in data)
            {
                var booking = new Booking
                {
                    BookingId = b.UnconfirmedBookingId,
                    Date      = b.Date,
                    Slot      = converter.ConvertDataSlotToWrapper(b.Slot),
                    Resource  = converter.ConvertDataResourceToWrapper(b.Resource),
                    User      = converter.ConvertDataUserToWrapper(b.User),
                    BookedBy  = converter.ConvertDataUserToWrapper(b.BookedBy),
                };
                bookings.Add(booking);
            }
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

            var newBooking = db.Booking.Where(b => b.User.UserId == user.UserId &&
                                                b.Slot.SlotId == slot.SlotId &&
                                                b.Date == booking.Date).FirstOrDefault();
            if (newBooking == null) newBooking = new DataLayer.Models.Booking();

            newBooking.Date     = booking.Date;
            newBooking.Slot     = slot;
            newBooking.Resource = db.Resources.Where(r => r.ResourceId == booking.Resource.ResourceId).FirstOrDefault();
            newBooking.User     = user;
            newBooking.BookedBy = user;

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
                    booking.BookedBy = user;

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

            //Add booking for Attendees
            foreach (string data in users)
            {
                var attendee = db.Users.Where(u => u.UserId == new Guid(data)).FirstOrDefault();
                AddUnconfirmedBooking(db, slotList, attendee, resource, date, user);
            }

            //Add booking for team memebers
            foreach (string data in teams)
            {
                var team = db.Teams.Where(u => u.TeamId == new Guid(data)).FirstOrDefault();
                foreach (DataLayer.Models.User member in team.Members)
                {
                    AddUnconfirmedBooking(db, slotList, member, resource, date, user);
                }
            }
                
            db.SaveChanges();
        }

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        public void AddUnconfirmedBooking(ReScrumEntities db, List<DataLayer.Models.Slot> slots, DataLayer.Models.User user, DataLayer.Models.Resource resource, DateTime date, DataLayer.Models.User bookedBy)
        {
            foreach (DataLayer.Models.Slot slot in slots)
            {
                var booking = new DataLayer.Models.UnconfirmedBooking();

                booking.Date           = date;
                booking.Slot           = slot;
                booking.Resource       = resource;
                booking.User           = user;
                booking.BookedBy       = bookedBy;

                db.UnconfirmedBooking.Add(booking);
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Gets an booking from the database
        /// </summary>
        /// <param name="BookingId">The booking id</param>
        /// <returns></returns>
        public Booking GetBooking(Guid? BookingId)
        {
            var db = new ReScrumEntities();

            var entry = db.Booking.Where(b => b.BookingId == BookingId).FirstOrDefault();

            var booking = new Booking
            {
                BookingId = entry.BookingId,
                Date      = entry.Date,
                Slot      = converter.ConvertDataSlotToWrapper(entry.Slot),
                Resource  = converter.ConvertDataResourceToWrapper(entry.Resource),
                User      = converter.ConvertDataUserToWrapper(entry.User),
                BookedBy  = converter.ConvertDataUserToWrapper(entry.BookedBy),
            };

            return booking;

        }

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        public void ConfirmBooking(Guid? unconfirmedBookingId)
        {
            var db = new ReScrumEntities();

            var unconfirmedBooking = db.UnconfirmedBooking.Where(b => b.UnconfirmedBookingId == unconfirmedBookingId).FirstOrDefault();

            var newBooking = db.Booking.Where(b => b.User.UserId == unconfirmedBooking.User.UserId &&
                                                b.Slot.SlotId == unconfirmedBooking.Slot.SlotId &&
                                                b.Date == unconfirmedBooking.Date).FirstOrDefault();
            if (newBooking == null) newBooking = new DataLayer.Models.Booking();

            newBooking.Date     = unconfirmedBooking.Date;
            newBooking.Slot     = unconfirmedBooking.Slot;
            newBooking.Resource = unconfirmedBooking.Resource;
            newBooking.User     = unconfirmedBooking.User;
            newBooking.BookedBy = unconfirmedBooking.BookedBy;

            if (newBooking.BookingId == null) db.Booking.Add(newBooking);

            //Remove the unconfirmed booking
            db.UnconfirmedBooking.Remove(unconfirmedBooking);

            db.SaveChanges();
        }

        public DateTime FindStartDate()
        {
            var date = DateTime.Today;

            switch (date.DayOfWeek)
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

            return date;
        }
    }
}
