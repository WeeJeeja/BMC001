using DataLayer;
using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IBookingService
    {
        /// <summary>
        /// Gets of all of a users bookings for a week
        /// </summary>
        /// <param name="userId">The user</param>
        /// <returns>A list of bookings</returns>
        List<Booking> GetThisWeeksBookings(Guid? userId);

        /// <summary>
        /// Gets all of a resources bookings for a week
        /// </summary>
        /// <param name="resourceId">The resource</param>
        /// <param name="date">The date</param>
        /// <returns>A list of bookings</returns>
        List<Booking> GetThisWeeksBookingsForAResource(Guid? resourceId, DateTime date);

        /// <summary>
        /// Gets all of a users unconfirmed bookings for a week
        /// </summary>
        /// <param name="userId">The users</param>
        /// <returns>A list of unconfirmed bookins</returns>
        List<Booking> GetThisWeeksUnconfirmedBookings(Guid? userId);

        /// <summary>
        /// Gets a list of resources that are available on the given date and time
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="time">The time slot</param>
        /// <returns>A list of available resources</returns>
        List<Resource> GetAvailableResources(DateTime date, Guid? time);

        /// <summary>
        /// Gets a list of resources that are avalable in given a date and time range
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="startSlot">The start time</param>
        /// <param name="endSlot">The end time</param>
        /// <returns>A list of available reosurces</returns>
        List<Resource> GetAvailableResourcesForBlockBooking(DateTime startDate, DateTime endDate, Guid? startSlot, Guid? endSlot);

        /// <summary>
        /// Gets a list of resources that have at least the capacity given and are available on a given date and time
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="startSlot">The start time</param>
        /// <param name="endSlot">The end time</param>
        /// <param name="capacity">The capcity</param>
        /// <returns></returns>
        List<Resource> GetAvailableResourcesForGroupBooking(DateTime date, Guid? startSlot, Guid? endSlot, int capacity);

        /// <summary>
        /// Adds a booking to the database
        /// </summary>
        /// <param name="booking">The booking to be added</param>
        void AddBooking(Booking booking);

        /// <summary>
        /// Adds a block of bookings to the database
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="resourceId">The resource</param>
        /// <param name="userId">The user</param>
        void AddBlockBooking(DateTime startDate, DateTime endDate, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId);

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
        void AddGroupBooking(DateTime date, List<string> users, List<string> teams, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId);

        /// <summary>
        /// Adds an uncomfirmed booking for to the database
        /// </summary>
        /// <param name="db">The database instance</param>
        /// <param name="slots">The time slots for the booking</param>
        /// <param name="user">The user</param>
        /// <param name="resource">The resource</param>
        /// <param name="date">The date</param>
        /// <param name="bookedBy">The person who made the group booking</param>
        void AddUnconfirmedBooking(ReScrumEntities db, List<DataLayer.Models.Slot> slots, DataLayer.Models.User user, DataLayer.Models.Resource resource, DateTime date, DataLayer.Models.User bookedBy);

        /// <summary>
        /// Gets a booking from the database
        /// </summary>
        /// <param name="BookingId">The booking Id</param>
        /// <returns>The booking</returns>
        Booking GetBooking(Guid? BookingId);

        /// <summary>
        /// Removed the unconformed booking and adds it to the booking table
        /// </summary>
        /// <param name="unconfirmedBookingId">The unconfirmed booking</param>
        void ConfirmBooking(Guid? unconfirmedBookingId);

        /// <summary>
        /// Deletes a booking from the database
        /// </summary>
        /// <param name="bookingId">The id of the booking to be removed</param>
        void DeleteBooking(Guid? bookingId);

        /// <summary>
        /// Adds attendees to an existing group booking
        /// </summary>
        /// <param name="bookingId">The booking the users should be added to</param>
        /// <param name="userId">The attendee to be added</param>
        void AddAttendeeToGroupBooking(Guid? bookingId, Guid? userId);

        /// <summary>
        /// Adds bookings for the users for all of the time slots not already booked
        /// </summary>
        /// <param name="date">The start date</param>
        /// <param name="userId">The user</param>
        /// <returns>True if a resource is avaible and booking was successful, false otherwise</returns>
        bool AutoBook(DateTime date, Guid? userId);
    }
}
