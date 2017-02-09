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

        /// <summary>
        /// Gets a list of all bookings for a particular user and week
        /// </summary>
        /// <returns>Returns a list of all of a user's bookings for a particular week</returns>
        public List<Booking> GetThisWeeksBookings(Guid? userId)
        {
            //work out the start of the week
            var date = DateTime.Today;
            while (!date.DayOfWeek.Equals("Monday"))
            {
                date = date.AddDays(-1);
            }

            //db connection
            var db = new ReScrumEntities();

            //get this weeks bookings
            var data = db.Booking.Where(b => b.Date >= date && b.Date < date.AddDays(7)).ToList();

            //the booking s that belong to the user
            data = data.Where(u => u.User.UserId == userId).ToList();

            var bookings = new List<Booking>();

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
        /// Adds a new resource to the database
        /// </summary>
        /// <param name="resource">The new resource to be added</param>
        public void AddResource(Resource resource)
        {
            var db = new ReScrumEntities();

            var newResource = new DataLayer.Models.Resource
            {
                ResourceId = resource.ResourceId,
                Name = resource.Name,
                Description = resource.Description,
                Category = resource.Category,
                Capacity = resource.Capacity,
                Location = resource.Location,
            };

            db.Resources.Add(newResource);

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
