using DataLayer;
using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IBookingService
    {
        List<Resource> GetAvailableResources(DateTime date, Guid? time);

        /// <summary>
        /// Gets a list of all unconfirmed bookings for a particular user and week
        /// </summary>
        /// <returns>Returns a list of all of a user's unconfirmed bookings for a particular week</returns>
        List<Booking> GetThisWeeksUnconfirmedBookings(Guid? userId);

        System.Collections.Generic.List<Booking> GetThisWeeksBookings(Guid? userId);
        List<Resource> GetAvailableResourcesForBlockBooking(DateTime startDate, DateTime endDate, Guid? startSlot, Guid? endSlot);

        void AddBlockBooking(DateTime startDate, DateTime endDate, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId);

        List<Resource> GetAvailableResourcesForGroupBooking(DateTime date, Guid? startSlot, Guid? endSlot, int capacity);
        
        void AddBooking(Booking booking);

        
        void AddGroupBooking(DateTime date, List<string> users, List<string> teams, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId);

        /// <summary>
        /// Gets an unconfirmed booking from the database
        /// </summary>
        /// <param name="unconfirmedBookingId">The unconfirmed booking id</param>
        /// <returns></returns>
        Booking GetUnconfirmedBooking(Guid? unconfirmedBookingId);

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        void AddUnconfirmedBooking(ReScrumEntities db, List<DataLayer.Models.Slot> slots, DataLayer.Models.User user, DataLayer.Models.Resource resource, DateTime date, DataLayer.Models.User bookedBy);

        /// <summary>
        /// Adds a new booking to the database
        /// </summary>
        /// <param name="resource">The new booking to be added</param>
        void ConfirmBooking(Guid? unconfirmedBookingId);
    }
}
