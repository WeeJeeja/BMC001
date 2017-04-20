using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IRateService
    {
        /// <summary>
        /// Calculates the number of slots (hours) the resource was in use
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource</param>
        /// <returns>The number of slots the resource was booked for during the date range entered</returns>
        float CalculateResourceFrequencyRate(DateTime startDate, DateTime endDate, Guid? resourceId);

        /// <summary>
        /// Calculates the frequency rate for an individual resource between a given date range
        /// Frequency rate: percentage of time space is used compared to its availability
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the frequency rate for</param>
        /// <returns>The frequency rate for the resource</returns>
        float CalculateResourceOccupancyRate(DateTime startDate, DateTime endDate, Guid? resourceId);

        /// <summary>
        /// Calculates the occupnacy rate for an individual resource between a given date range
        /// Occupancy rate: how full the space is compared to its capacity
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the occupancy rate for</param>
        /// <returns>The occupancy rate for the resource</returns>
        float CalculateResourceUtilisationRate(DateTime startDate, DateTime endDate, Guid? resourceId);

        /// <summary>
        /// Calculates the utilisation rate for an individual resource between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the utilisation rate for</param>
        /// <returns>The utilisation rate for the resource</returns>
        List<DataLayer.Models.Booking> GetSlotsUtilised(DateTime startDate, DateTime endDate, Guid? resourceId);

        /// <summary>
        /// Calculates the utilisation rate for the compnay between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <returns>The utilisation rate for the company</returns>
        float CalculateUtilisationRate(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Calculates the frequency rate for the company between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <returns>The frequency rate for the company</returns>
        float CalculateFrequencyRate(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Calculates the occupnacy rate for the company between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <returns>The occupancy rate for the company</returns>
        float CalculateOccupancyRate(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Counts how many resources were in use on a certain day at a certain time
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="slotId">The time slot</param>
        /// <returns>The number of resources used in a time slot</returns>
        int GetResourcesUsedInSlot(DateTime date, Guid? slotId);

        /// <summary>
        /// Calculates the frequency rate for a time slot between a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="slotId">The time slot</param>
        /// <returns>The frequency rate for the time slot</returns>
        float CalculateSlotFrequencyRate(DateTime date, Guid? slotId);

        /// <summary>
        /// Calculates the occupnacy rate for a slot time for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="slotId">The slot time</param>
        /// <returns>The occupancy rate for the time slot</returns>
        float CalculateSlotOccupancyRate(DateTime date, Guid? slotId);

        /// <summary>
        /// Calculates the utilisation rate for a time slot for a given date
        /// </summary>
        /// <param name="startDate">The date</param>
        /// <param name="endDate">The time slot</param>
        /// <returns>The utilisation rate for the time slot</returns>
        float CalculateSlotUtilisationRate(DateTime date, Guid? slotId);
    }
}
