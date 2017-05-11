using DataLayer;
using DomainLayer.WrapperModels;
using HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer
{
    /// <summary>
    /// Booking service to retrive, add and delete bookings from the database
    /// </summary>
    public class BookingService : IBookingService
    {
        #region Fields

        ModelConversitions converter     = new ModelConversitions();
        //ISlotService slotService         = new SlotService();

        #endregion

        /// <summary>
        /// Gets of all of a users bookings for a week
        /// </summary>
        /// <param name="userId">The user</param>
        /// <returns>A list of bookings</returns>
        public List<Booking> GetThisWeeksBookings(Guid? userId)
        {
            //work out the start of the week
            var date = FindStartDate(DateTime.Today);

            var endDate = date.AddDays(5);

            //db connection
            var db = new ReScrumEntities();

            //the bookings that belong to the user
            var data = db.Booking.Where(u => u.User.UserId == userId).ToList();

            var bookings = new List<Booking>();

            //if there are no bookings return the empty list
            if (data.Count < 1) return bookings;

            //get this weeks bookings
            data = data.Where(b => b.Date >= date && b.Date < endDate).ToList();

            var bookingEntries = converter.ConvertDataBookingListToWrapper(data);

            foreach (Booking entry in bookingEntries)
            {
                bookings.Add(entry);
            }

            return bookings;
        }

        /// <summary>
        /// Gets all of a resources bookings for a week
        /// </summary>
        /// <param name="resourceId">The resource</param>
        /// <param name="date">The date</param>
        /// <returns>A list of bookings</returns>
        public List<Booking> GetThisWeeksBookingsForAResource(Guid? resourceId, DateTime date)
        {
            //work out the start of the week
            var startDate = FindStartDate(date);

            var endDate = date.AddDays(5);

            //db connection
            var db = new ReScrumEntities();

            //the bookings that belong to the user
            var data = db.Booking.Where(u => u.Resource.ResourceId == resourceId).ToList();

            var bookings = new List<Booking>();
            if (data.Count < 1) return bookings;

            //get this weeks bookings
            data = data.Where(b => b.Date >= startDate && b.Date < endDate).ToList();

            var bookingEntries = converter.ConvertDataBookingListToWrapper(data);

            foreach (Booking entry in bookingEntries)
            {
                bookings.Add(entry);
            }
            
            return bookings;
        }

        /// <summary>
        /// Gets all of a users unconfirmed bookings for a week
        /// </summary>
        /// <param name="userId">The users</param>
        /// <returns>A list of unconfirmed bookins</returns>
        public List<Booking> GetThisWeeksUnconfirmedBookings(Guid? userId)
        {
            //work out the start of the week
            var date = FindStartDate(DateTime.Today);

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

        /// <summary>
        /// Gets a list of resources that are available on the given date and time
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="time">The time slot</param>
        /// <returns>A list of available resources</returns>
        public List<Resource> GetAvailableResources(DateTime date, Guid? time)
        {
            var db = new ReScrumEntities();

            //get all resources that are booked the the given date and time
            var unavailableResources  = db.Booking.Where(b =>
                    b.Date            == date &&
                    b.Slot.SlotId     == time).Select(r => r.Resource).ToList();

            //get all resources except those that are already booked
            var availableResources = db.Resources.Where(r => r.CancellationDate == null).ToList()
                                        .Except(unavailableResources).ToList();

            var resources = converter.ConvertDataResourceListToWrapper(availableResources);

            return resources.ToList(); ;
        }

        /// <summary>
        /// Gets a list of resources that are avalable in given a date and time range
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="startSlot">The start time</param>
        /// <param name="endSlot">The end time</param>
        /// <returns>A list of available reosurces</returns>
        public List<Resource> GetAvailableResourcesForBlockBooking(DateTime startDate, DateTime endDate, Guid? startSlot, Guid? endSlot)
        {
            var db = new ReScrumEntities();

            var slotService = new SlotService();
            var startTime = slotService.GetSlot(startSlot);
            var endTime = slotService.GetSlot(endSlot);

            //get all resources that are booked for the given date and time range
            var unavailableResources = db.Booking.Where(b =>
                    b.Date >= startDate &&
                    b.Date <= endDate &&
                    b.Slot.StartTime >= startTime.StartTime &&
                    b.Slot.EndTime <= endTime.EndTime).Select(r => r.Resource).ToList();

            //get all resources except those that are already booked
            var availableResources = db.Resources.Where(r => r.CancellationDate == null).
                                        ToList().Except(unavailableResources).ToList();

            var resources = converter.ConvertDataResourceListToWrapper(availableResources);

            return resources.ToList(); ;
        }

        /// <summary>
        /// Gets a list of resources that have at least the capacity given and are available on a given date and time
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="startSlot">The start time</param>
        /// <param name="endSlot">The end time</param>
        /// <param name="capacity">The capcity</param>
        /// <returns></returns>
        public List<Resource> GetAvailableResourcesForGroupBooking(DateTime date, Guid? startSlot, Guid? endSlot, int capacity)
        {
            var db = new ReScrumEntities();

            var slotService = new SlotService();
            var startTime = slotService.GetSlot(startSlot);
            var endTime = slotService.GetSlot(endSlot);

            //get all resources that are booked for a given date and a time range
            var unavailableResources = db.Booking.Where(b =>
                    b.Date == date &&
                    b.Slot.StartTime >= startTime.StartTime &&
                    b.Slot.EndTime <= endTime.EndTime).Select(r => r.Resource).ToList();

            //get all resources except those that are already booked
            var availableResources = db.Resources.Where(r => r.CancellationDate == null)
                                        .ToList().Except(unavailableResources).ToList();

            availableResources.RemoveAll(r => r.Capacity < capacity);

            var resources = converter.ConvertDataResourceListToWrapper(availableResources);

            return resources.ToList(); ;
        }

        /// <summary>
        /// Adds a booking to the database
        /// </summary>
        /// <param name="booking">The booking to be added</param>
        public void AddBooking(Booking booking)
        {
            var db = new ReScrumEntities();

            var slot = db.Slots.Where( s => s.SlotId == booking.Slot.SlotId).FirstOrDefault();
            var user = db.Users.Where(u => u.UserId == booking.User.UserId).FirstOrDefault();

            //check if a current booking exist for the user, date and time slot
            var newBooking = db.Booking.Where(b => b.User.UserId == user.UserId &&
                                                b.Slot.SlotId == slot.SlotId &&
                                                b.Date == booking.Date).FirstOrDefault();

            //if not create a new booking parameter
            if (newBooking == null) newBooking = new DataLayer.Models.Booking();

            //set/update the booking values
            newBooking.Date     = booking.Date;
            newBooking.Slot     = slot;
            newBooking.Resource = db.Resources.Where(r => r.ResourceId == booking.Resource.ResourceId).FirstOrDefault();
            newBooking.User     = user;
            newBooking.BookedBy = user;

            //if it is a new booking add it (creating new guid id), else the exisitng booking will be updated => no double booking
            if (newBooking.BookingId == null)  db.Booking.Add(newBooking);

            db.SaveChanges();
        }

        /// <summary>
        /// Adds a block of bookings to the database
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="resourceId">The resource</param>
        /// <param name="userId">The user</param>
        public void AddBlockBooking(DateTime startDate, DateTime endDate, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId)
        {
            var db = new ReScrumEntities();

            var startSlot = db.Slots.Where(s => s.SlotId == startTime).FirstOrDefault();
            var endSlot   = db.Slots.Where(s => s.SlotId == endTime).FirstOrDefault();
            var user      = db.Users.Where(u => u.UserId == userId).FirstOrDefault();
            var resource  = db.Resources.Where(r => r.ResourceId == resourceId).FirstOrDefault();

            var slotList = db.Slots.Where(s => s.StartTime >= startSlot.StartTime &&
                                                s.EndTime <= endSlot.EndTime).ToList();

            var date = startDate;
            //for every date in the date range given
            while (date <= endDate)
            {
                foreach (DataLayer.Models.Slot slot in slotList)
                {
                    //check if a booking exist for the user on the given date and time
                    var booking = db.Booking.Where(b => b.User.UserId == user.UserId &&
                                                b.Slot.SlotId == slot.SlotId &&
                                                b.Date == date).FirstOrDefault();
                    //if no booking exists create a new booking object
                    if (booking == null) booking = new DataLayer.Models.Booking();
                    
                    //Add or update booking values
                    booking.Date     = date;
                    booking.Slot     = slot;
                    booking.Resource = resource;
                    booking.User     = user;
                    booking.BookedBy = user;

                    //if a new booking was created add it to the database(giving a new guid id), else update the existing booking
                    if (booking.BookingId == null) db.Booking.Add(booking);
                }
                date = date.AddDays(1);
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Calls the unconfirmed booking method for each attendee of the group booking
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="users">The attendees</param>
        /// <param name="teams">The teams</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="resourceId">The resource</param>
        /// <param name="userId">The user</param>
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
        /// Adds an uncomfirmed booking for to the database
        /// </summary>
        /// <param name="db">The database instance</param>
        /// <param name="slots">The time slots for the booking</param>
        /// <param name="user">The user</param>
        /// <param name="resource">The resource</param>
        /// <param name="date">The date</param>
        /// <param name="bookedBy">The person who made the group booking</param>
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
        /// Gets a booking from the database
        /// </summary>
        /// <param name="BookingId">The booking Id</param>
        /// <returns>The booking</returns>
        public Booking GetBooking(Guid? BookingId)
        {
            var db = new ReScrumEntities();

            var entry = db.Booking.Where(b => b.BookingId == BookingId).FirstOrDefault();

            var booking = new Booking
            {
                BookingId    = entry.BookingId,
                Date         = entry.Date,
                Slot         = converter.ConvertDataSlotToWrapper(entry.Slot),
                Resource     = converter.ConvertDataResourceToWrapper(entry.Resource),
                User         = converter.ConvertDataUserToWrapper(entry.User),
                BookedBy     = converter.ConvertDataUserToWrapper(entry.BookedBy),
                GroupBooking = entry.GroupBooking,
            };

            //if the booking is for a group the attendees need to be retrieved
            if (booking.GroupBooking == true)
            {
                //get all confirmed attendees
                var confirmedBookings = db.Booking.Where(b => b.Date == entry.Date &&
                                                    b.Slot.SlotId == entry.Slot.SlotId &&
                                                    b.Resource.ResourceId == entry.Resource.ResourceId).ToList();

                var confirmedAttendees = confirmedBookings.Select(u => u.User).ToList();
                booking.ConfirmedAttendees = converter.ConvertDataUserListToWrapper(confirmedAttendees);

                //get all unconfirmed attendees
                var unconfirmedBookings = db.UnconfirmedBooking.Where(b => b.Date == entry.Date &&
                                                    b.Slot.SlotId == entry.Slot.SlotId &&
                                                    b.Resource.ResourceId == entry.Resource.ResourceId).ToList();
                var unconfirmedAttendees = unconfirmedBookings.Select(u => u.User).ToList();
                booking.UnconfirmedAttendees = converter.ConvertDataUserListToWrapper(unconfirmedAttendees);
            }

            return booking;

        }

        /// <summary>
        /// Removed the unconformed booking and adds it to the booking table
        /// </summary>
        /// <param name="unconfirmedBookingId">The unconfirmed booking</param>
        public void ConfirmBooking(Guid? unconfirmedBookingId)
        {
            var db = new ReScrumEntities();

            //get the unconfirmed booking
            var unconfirmedBooking = db.UnconfirmedBooking.Where(b => b.UnconfirmedBookingId == unconfirmedBookingId).FirstOrDefault();

            //check to see if the user already has a booking for the date and time given
            var newBooking = db.Booking.Where(b => b.User.UserId == unconfirmedBooking.User.UserId &&
                                                b.Slot.SlotId == unconfirmedBooking.Slot.SlotId &&
                                                b.Date == unconfirmedBooking.Date).FirstOrDefault();

            //if no booking exists create a new bookng object, else the exisitng booking will be updated
            if (newBooking == null) newBooking = new DataLayer.Models.Booking();

            //add or update booking object
            newBooking.Date         = unconfirmedBooking.Date;
            newBooking.Slot         = unconfirmedBooking.Slot;
            newBooking.Resource     = unconfirmedBooking.Resource;
            newBooking.User         = unconfirmedBooking.User;
            newBooking.BookedBy     = unconfirmedBooking.BookedBy;
            newBooking.GroupBooking = true;

            //if it is a new booking add it to the database (give a new guid id), else update the existing booking
            if (newBooking.BookingId == null) db.Booking.Add(newBooking);

            //Remove the unconfirmed booking
            db.UnconfirmedBooking.Remove(unconfirmedBooking);

            db.SaveChanges();
        }

        /// <summary>
        /// Deletes a booking from the database
        /// </summary>
        /// <param name="bookingId">The id of the booking to be removed</param>
        public void DeleteBooking(Guid? bookingId)
        {
            var db = new ReScrumEntities();

            var booking = db.Booking.Where(b => b.BookingId == bookingId).FirstOrDefault();

            db.Booking.Remove(booking);

            db.SaveChanges();
        }

        /// <summary>
        /// Adds attendees to an existing group booking
        /// </summary>
        /// <param name="bookingId">The booking the users should be added to</param>
        /// <param name="userId">The attendee to be added</param>
        public void AddAttendeeToGroupBooking(Guid? bookingId, Guid? userId)
        {
            var db = new ReScrumEntities();

            var booking = db.Booking.Where(b => b.BookingId == bookingId).FirstOrDefault();

            var slots = new List<DataLayer.Models.Slot>();
            slots.Add(booking.Slot);

            var attendee = db.Users.Where(u => u.UserId == userId).FirstOrDefault();
            AddUnconfirmedBooking(db, slots, attendee, booking.Resource, booking.Date, booking.BookedBy);

            db.SaveChanges();
        }

        /// <summary>
        /// Adds bookings for the users for all of the time slots not already booked
        /// </summary>
        /// <param name="date">The start date</param>
        /// <param name="userId">The user</param>
        /// <returns>True if a resource is avaible and booking was successful, false otherwise</returns>
        public bool AutoBook(DateTime date, Guid? userId)
        {
            var db = new ReScrumEntities();

            //get the user
            var user = db.Users.Where(u => u.UserId == userId).FirstOrDefault();

            //get all active slots
            var slotList = db.Slots.Where(s => s.CancellationDate == null).ToList();

            //find the end date
            var endDate = date.AddDays(4);

            //find the start time slot
            var startSlot = slotList.First();
            foreach (DataLayer.Models.Slot slot in slotList)
            {
                if (startSlot.StartTime.CompareTo(slot.StartTime) == 1)
                {
                    startSlot = slot;
                }
            }

            //find the end time slot
            var endSlot = slotList.Last();
            foreach (DataLayer.Models.Slot slot in slotList)
            {
                if (endSlot.EndTime.CompareTo(slot.EndTime) == -1 )
                {
                    endSlot = slot;
                }
            }

            //find resource that is available all week
            var resourceList = GetAvailableResourcesForBlockBooking(date, endDate, startSlot.SlotId, endSlot.SlotId).ToList();

            //get the resource with the smallest capacity
            if (resourceList.Count < 1) return false;
            var resource = resourceList.OrderBy(r => r.Capacity).First();

            //add a booking for every time slot that is not already booked in the week => does NOT override existing bookings
            while (date <= endDate)
            {
                foreach (DataLayer.Models.Slot slot in slotList)
                {
                    var booking = db.Booking.Where(b => b.User.UserId == user.UserId &&
                                                b.Slot.SlotId == slot.SlotId &&
                                                b.Date == date).FirstOrDefault();

                    //Do not want to override existing bookings in auto functionality
                    if (booking == null)
                    {
                        booking = new DataLayer.Models.Booking()
                        {
                            Date     = date,
                            Slot     = slot,
                            Resource = db.Resources.Where(r => r.ResourceId == resource.ResourceId).FirstOrDefault(),
                            User     = user,
                            BookedBy = user,
                        };
                        db.Booking.Add(booking);
                    }
                }
                date = date.AddDays(1);
            }

            db.SaveChanges();
            return true;
        }

        #region HelperMethods

        /// <summary>
        /// Finds the week starting date for given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime FindStartDate(DateTime date)
        {
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

        #endregion
    }
}
