using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IBookingService
    {
        List<Resource> GetAvailableResources(DateTime date, Guid? time);

        System.Collections.Generic.List<Booking> GetThisWeeksBookings(Guid? userId);
        List<Resource> GetAvailableResourcesForBlockBooking(DateTime startDate, DateTime endDate, Guid? startSlot, Guid? endSlot);

        void AddBlockBooking(DateTime startDate, DateTime endDate, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId);

        List<Resource> GetAvailableResourcesForGroupBooking(DateTime date, Guid? startSlot, Guid? endSlot, int capacity);
        
        void AddBooking(Booking booking);

        void AddGroupBooking(DateTime date, List<string> users, List<string> teams, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId);
        
    }
}
