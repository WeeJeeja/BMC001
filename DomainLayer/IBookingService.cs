using System;
namespace DomainLayer
{
    public interface IBookingService
    {
        void AddResource(DomainLayer.WrapperModels.Resource resource);
        void DeleteResource(Guid? resourceId);
        DomainLayer.WrapperModels.Resource GetResource(Guid resourceId);
        System.Collections.Generic.List<DomainLayer.WrapperModels.Booking> GetThisWeeksBookings(Guid? userId);
        void UpdateResource(DomainLayer.WrapperModels.Resource data);
    }
}
