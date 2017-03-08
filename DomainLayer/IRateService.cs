using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    interface IRateService
    {
        float CalculateResourceFrequencyRate(DateTime startDate, DateTime endDate, Resource resource);
        float CalculateResourceOccupancyRate(DateTime startDate, DateTime endDate, Resource resource);
        float CalculateResourceUtilisationRate(DateTime startDate, DateTime endDate, Resource resource);
        List<DataLayer.Models.Booking> GetSlotsUtilised(DateTime startDate, DateTime endDate, Resource resource);
    }
}
