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
    public class RateService : IRateService
    {
        ModelConversitions converter = new ModelConversitions();

        /// <summary>
        /// Calculates the number of slots (hours) the resource was in use
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource</param>
        /// <returns>The number of slots the resource was booked for during the date range entered</returns>
        public List<DataLayer.Models.Booking> GetSlotsUtilised(DateTime startDate, DateTime endDate, Guid? resourceId)
        {
            var db = new ReScrumEntities();

            var bookings = db.Booking.Where(r => r.Resource.ResourceId == resourceId &&
                                            r.Date >= startDate &&
                                            r.Date <= endDate).ToList();
            return bookings;
        }

        /// <summary>
        /// Calculates the frequency rate for an individual resource between a given date range
        /// Frequency rate: percentage of time space is used compared to its availability
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the frequency rate for</param>
        /// <returns>The frequency rate for the resource</returns>
        public float CalculateResourceFrequencyRate(DateTime startDate, DateTime endDate, Guid? resourceId)
        {
            var db = new ReScrumEntities();

            var resource = db.Resources.Where(r => r.ResourceId == resourceId).FirstOrDefault();

            //Calculate how many days
            var dateRange = endDate.DayOfYear - startDate.DayOfYear + 1;

            //Resource availability
            var slots        = db.Slots.Count();
            var availability = slots * dateRange;

            //How many times the resource was used
            var hoursUsed = GetSlotsUtilised(startDate, endDate, resource.ResourceId).Count();

            //Number of slots the space was in use/Number of slots available = frequency rate %
            var frequencyRate = ((float)hoursUsed / (float)availability);

            if (float.IsNaN(frequencyRate)) return 0;

            return frequencyRate;
        }

        /// <summary>
        /// Calculates the occupnacy rate for an individual resource between a given date range
        /// Occupancy rate: how full the space is compared to its capacity
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the occupancy rate for</param>
        /// <returns>The occupancy rate for the resource</returns>
        public float CalculateResourceOccupancyRate(DateTime startDate, DateTime endDate, Guid? resourceId)
        {
            var db = new ReScrumEntities();

            //Resource capacity
            var resource   = db.Resources.Where(r => r.ResourceId == resourceId).FirstOrDefault();
            float capacity = resource.Capacity;

            //Total number of people occupaying resource
            var scheduleEntries = GetSlotsUtilised(startDate, endDate, resourceId);
            float occupants = scheduleEntries.Count();

            //Number of hours resource was in use
            var date = startDate;
            float hoursResourceWasInUse = 0;
            while (date <= endDate)
            {
                var hoursUsedInDay = scheduleEntries.Where(b => b.Date == date).ToList();
                hoursResourceWasInUse += (float)hoursUsedInDay.Count();
                date = date.AddDays(1);
            }

            //Occupancy rate = occupants / (capacity * hoursResourceWasInUse)
            var occupancyRate = (occupants / (capacity * hoursResourceWasInUse));

            if (float.IsNaN(occupancyRate)) return 0;

            return occupancyRate;
        }

        /// <summary>
        /// Calculates the utilisation rate for an individual resource between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the utilisation rate for</param>
        /// <returns>The utilisation rate for the resource</returns>
        public float CalculateResourceUtilisationRate(DateTime startDate, DateTime endDate, Guid? resourceId)
        {
            //Utilisation rate: frequency rate * occupancy rate
            var frequencyRate = CalculateResourceFrequencyRate(startDate, endDate, resourceId);
            var occupancyRate = CalculateResourceOccupancyRate(startDate, endDate, resourceId);

            var utilisationRate = frequencyRate * occupancyRate;


            if (float.IsNaN(utilisationRate)) return 0;
            return utilisationRate;
        }

        /// <summary>
        /// Calculates the utilisation rate for the compnay between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <returns>The utilisation rate for the company</returns>
        public float CalculateUtilisationRate(DateTime startDate, DateTime endDate)
        {
            var db        = new ReScrumEntities();
            var resources = db.Resources.Where(r => r.CancellationDate == null).ToList();

            //Utilisation rate: frequency rate * occupancy rate
            float utilisationRate = 0;
            foreach(DataLayer.Models.Resource resource in resources)
            {
                utilisationRate += CalculateResourceUtilisationRate(startDate, endDate, resource.ResourceId);
            }
            
            if (float.IsNaN(utilisationRate)) return 0;
            utilisationRate = utilisationRate / (float)resources.Count();
            return utilisationRate;
        }

        /// <summary>
        /// Calculates the frequency rate for the company between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <returns>The frequency rate for the company</returns>
        public float CalculateFrequencyRate(DateTime startDate, DateTime endDate)
        {
            var db        = new ReScrumEntities();
            var resources = db.Resources.Where(r => r.CancellationDate == null).ToList();

            /// Frequency rate: percentage of time space is used compared to its availability
            float frequencyRate = 0;

            foreach (DataLayer.Models.Resource resource in resources)
            {
                frequencyRate += CalculateResourceFrequencyRate(startDate, endDate, resource.ResourceId);
            }
            if (float.IsNaN(frequencyRate)) return 0;
            frequencyRate = frequencyRate / (float)resources.Count();
            return frequencyRate;
        }

        /// <summary>
        /// Calculates the occupnacy rate for the company between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <returns>The occupancy rate for the company</returns>
        public float CalculateOccupancyRate(DateTime startDate, DateTime endDate)
        {
            var db        = new ReScrumEntities();
            var resources = db.Resources.Where(r => r.CancellationDate == null).ToList();

            /// Occupancy rate: how full the space is compared to its capacity
            float occupancyRate = 0;
            float resourcesUsedInDateRange = 0;
            foreach(DataLayer.Models.Resource resource in resources)
            {
                //only calculate the occupany rate if the resource was used in the date range
                var bookings = GetSlotsUtilised(startDate, endDate, resource.ResourceId);
                if (bookings.Count > 0)
                {
                    occupancyRate += CalculateResourceOccupancyRate(startDate, endDate, resource.ResourceId);
                    resourcesUsedInDateRange++;
                }
            }

            if (float.IsNaN(occupancyRate) || occupancyRate == 0.0) return 0;
            occupancyRate = occupancyRate / resourcesUsedInDateRange;
            return occupancyRate;
        }

        #region Rates for time slot

        /// <summary>
        /// Counts how many resources were in use on a certain day at a certain time
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="slotId">The time slot</param>
        /// <returns>The number of resources used in a time slot</returns>
        public float GetResourcesUsedInSlot(DateTime date, Guid? slotId)
        {
            var db = new ReScrumEntities();

            //All of the bookings for the time on the day -> all 9am booking for Tuesday 18th April
            var bookings = db.Booking.Where(b => b.Date == date && b.Slot.SlotId == slotId).ToList();
            if (bookings.Count() < 1) return 0;

            //Removes multiple bookings for the same resource -> group bookings
            var resourcesBookedInSlot = bookings.GroupBy(x => x.Resource).Select(y => y.First()).Count();

            return (float)resourcesBookedInSlot;
        }

        /// <summary>
        /// Calculates the frequency rate for a time slot between a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="slotId">The time slot</param>
        /// <returns>The frequency rate for the time slot</returns>
        public float CalculateSlotFrequencyRate(DateTime date, Guid? slotId)
        {
            var db = new ReScrumEntities();

            /// Frequency rate: percentage of time space is used compared to its availability
            float frequencyRate = 0;

            //Removes multiple bookings for the same resource -> group bookings
            var resourcesBookedInSlot = GetResourcesUsedInSlot(date, slotId);

            var resources = db.Resources.Where(r => r.CancellationDate == null || r.CancellationDate <= date).ToList();

            frequencyRate = (float)resourcesBookedInSlot / (float)resources.Count();

            if (float.IsNaN(frequencyRate)) return 0;
            return frequencyRate;
        }

        /// <summary>
        /// Calculates the occupnacy rate for a slot time for a given date
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="slotId">The slot time</param>
        /// <returns>The occupancy rate for the time slot</returns>
        public float CalculateSlotOccupancyRate(DateTime date, Guid? slotId)
        {
            var db = new ReScrumEntities();

            /// Occupancy rate: how full the space is compared to its capacity
            float occupancyRate = 0;

            //All of the bookings for the time on the day -> all 9am booking for Tuesday 18th April
            var bookings = db.Booking.Where(b => b.Date == date && b.Slot.SlotId == slotId).ToList();

            if (bookings.Count() < 1) return occupancyRate;

            //Get all the resources used in the bookings
            var resources = bookings.Select(b => b.Resource).ToList();

            foreach (DataLayer.Models.Resource resource in resources)
            {
                var totalOccupants = bookings.Where(r => r.Resource.ResourceId == resource.ResourceId).Count();

                occupancyRate += (float)totalOccupants/(float)resource.Capacity;
            }

            if (float.IsNaN(occupancyRate)) return 0;
            occupancyRate = occupancyRate / resources.Count();

            return occupancyRate;
        }

        /// <summary>
        /// Calculates the utilisation rate for a time slot for a given date
        /// </summary>
        /// <param name="startDate">The date</param>
        /// <param name="endDate">The time slot</param>
        /// <returns>The utilisation rate for the time slot</returns>
        public float CalculateSlotUtilisationRate(DateTime date, Guid? slotId)
        {
            var db = new ReScrumEntities();

            //Utilisation rate: frequency rate * occupancy rate
            float frequencyRate = CalculateSlotFrequencyRate(date, slotId);
            float occupancyRate = CalculateSlotOccupancyRate(date, slotId);

            var utilisationRate = frequencyRate * occupancyRate;

            return utilisationRate;
        }

        #endregion


    }
}
