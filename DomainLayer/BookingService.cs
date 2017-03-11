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
                    BookingId  = b.BookingId,
                    Date       = b.Date,
                    Slot       = converter.ConvertDataSlotToWrapper(b.Slot),
                    Resource   = converter.ConvertDataResourceToWrapper(b.Resource),
                    User       = converter.ConvertDataUserToWrapper(b.User),
                    Capacity   = b.Capacity,
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
                    b.Slot.TimeFormat >= startTime.TimeFormat &&
                    b.Slot.TimeFormat <= endTime.TimeFormat).Select(r => r.Resource).ToList();

            var availableResources = db.Resources.ToList().Except(unavailableResources).ToList();

            var resources = converter.ConvertDataResourceListToWrapper(availableResources);

            return resources.ToList(); ;
        }

        /// <summary>
        /// Gets the resource using the resourceId
        /// </summary>
        /// <returns>Returns the resource</returns>
        public Resource GetResource(Guid resourceId)
        {
            var db = new ReScrumEntities();

            var data = db.Resources.Where(u => u.ResourceId == resourceId).FirstOrDefault();

            var resource = new Resource
            {
                ResourceId  = data.ResourceId,
                Name        = data.Name,
                Description = data.Description,
                Category    = data.Category,
                Capacity    = data.Capacity,
                Location    = data.Location,
            };

            return resource;
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

            var newBooking = new DataLayer.Models.Booking
            {
                Date     = booking.Date,
                Slot     = slot,
                Resource = resource,
                User     = user,
                Capacity = booking.Capacity,
            };

            db.Booking.Add(newBooking);

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

            var slotList = db.Slots.Where(s => s.TimeFormat >= startSlot.TimeFormat &&
                                                s.TimeFormat <= endSlot.TimeFormat).ToList();

            // If run on October 20, 2006, the example produces the following output:
            //    CompareTo method returns 1: 10/20/2006 is later than 10/20/2005
            //    CompareTo method returns -1: 10/20/2006 is earlier than 10/20/2007
            var date = startDate;
            while (date <= endDate)
            {
                foreach (DataLayer.Models.Slot slot in slotList)
                {
                    var booking = new DataLayer.Models.Booking
                    {
                        Date     = date,
                        Slot     = slot,
                        Resource = resource,
                        User     = user,
                        Capacity = 1,
                    };

                    db.Booking.Add(booking);
                }
                date = date.AddDays(1);
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing resource
        /// </summary>
        /// <param name="data">The new resource details</param>
        public void UpdateResource(Resource data)
        {
            var db = new ReScrumEntities();

            var resource = db.Resources.Where(u => u.ResourceId == data.ResourceId).FirstOrDefault();

            resource.ResourceId  = data.ResourceId;
            resource.Name        = data.Name;
            resource.Description = data.Description;
            resource.Category    = data.Category;
            resource.Capacity    = data.Capacity;
            resource.Location    = data.Location;

            db.SaveChanges();
        }

        /// <summary>
        /// Deletes a resource from the database
        /// </summary>
        /// <param name="data">The resource to be deleted</param>
        public void DeleteResource(Guid? resourceId)
        {
            var db = new ReScrumEntities();

            var resource = db.Resources.Where(u => u.ResourceId == resourceId).FirstOrDefault();

            db.Resources.Remove(resource);

            db.SaveChanges();
        }
    }
}
