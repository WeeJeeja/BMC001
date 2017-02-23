using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IBookingService
    {
        void DeleteResource(Guid? resourceId);

        List<Resource> GetAvailableResources(DateTime date, string time);
        Resource GetResource(Guid resourceId);
        System.Collections.Generic.List<Booking> GetThisWeeksBookings(Guid? userId);
        void UpdateResource(Resource data);

        void AddBooking(Booking booking);
    }
}
