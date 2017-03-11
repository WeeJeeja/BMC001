using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IBookingService
    {
        void DeleteResource(Guid? resourceId);

        List<Resource> GetAvailableResources(DateTime date, Guid? time);
        Resource GetResource(Guid resourceId);
        System.Collections.Generic.List<Booking> GetThisWeeksBookings(Guid? userId);
        void UpdateResource(Resource data);
        List<Resource> GetAvailableResourcesForBlockBooking(DateTime startDate, DateTime endDate, Guid? startSlot, Guid? endSlot);

        void AddBlockBooking(DateTime startDate, DateTime endDate, Guid? startTime, Guid? endTime, Guid? resourceId, Guid? userId);
        
        void AddBooking(Booking booking);
    }
}
