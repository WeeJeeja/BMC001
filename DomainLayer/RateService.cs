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
        public List<DataLayer.Models.Booking> GetSlotsUtilised(DateTime startDate, DateTime endDate, Resource resource)
        {
            var db = new ReScrumEntities();

            var bookings = db.Booking.Where(r => r.Resource.ResourceId == resource.ResourceId &&
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
        public float CalculateResourceFrequencyRate(DateTime startDate, DateTime endDate, Resource resource)
        {
            var db = new ReScrumEntities();

            //Calculate how many days
            var dateRange = endDate.DayOfYear - startDate.DayOfYear + 1;

            //Resource availability
            var slots        = db.Slots.Count();
            var availability = slots * dateRange;

            //How many times the resource was used
            var hoursUsed = GetSlotsUtilised(startDate, endDate, resource).Count();

            //Number of slots the space was in use/Number of slots available = frequency rate %
            var frequencyRate = ((float)hoursUsed / (float)availability);

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
        public float CalculateResourceOccupancyRate(DateTime startDate, DateTime endDate, Resource resource)
        {
            //Calculate how many days
            var dateRange = endDate.DayOfYear - startDate.DayOfYear + 1;

            //Resource capacity
            float capacity = resource.Capacity;

            //Total number of people occupaying resource
            var scheduleEntries = GetSlotsUtilised(startDate, endDate, resource);

            float occupants = scheduleEntries.Sum(r => r.Capacity);

            Console.WriteLine(occupants + " employees have used this resource during this time period.");

            //Number of hours resource was in use
            float hoursResourceWasInUse = scheduleEntries.Count();
            Console.WriteLine("The resource has been booked for " + hoursResourceWasInUse + " hour(s) during said time period.");

            //Occupancy rate = occupants / (capacity * hoursResourceWasInUse)
            var occupancyRate = (occupants / (capacity * hoursResourceWasInUse));

            Console.WriteLine("The occupancy rate for " + resource.Name + " is: " + occupancyRate * 100 + "%");

            return occupancyRate;
        }

        /// <summary>
        /// Calculates the utilisation rate for an individual resource between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the utilisation rate for</param>
        /// <returns>The utilisation rate for the resource</returns>
        public float CalculateResourceUtilisationRate(DateTime startDate, DateTime endDate, Resource resource)
        {
            //Utilisation rate: frequency rate * occupancy rate
            var frequencyRate = CalculateResourceFrequencyRate(startDate, endDate, resource);
            var occupancyRate = CalculateResourceOccupancyRate(startDate, endDate, resource);

            var utilisationRate = frequencyRate * occupancyRate;

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
            var resources = db.Resources.ToList();

            //Utilisation rate: frequency rate * occupancy rate
            float frequencyRate   = 0;
            float occupancyRate   = 0;
            float utilisationRate = 0;

            foreach(DataLayer.Models.Resource data in resources)
            {
                var resource     = converter.ConvertDataResourceToWrapper(data);
                frequencyRate   += CalculateResourceFrequencyRate(startDate, endDate, resource);
                occupancyRate   += CalculateResourceOccupancyRate(startDate, endDate, resource);
                utilisationRate += frequencyRate * occupancyRate;
            }
            if (float.IsNaN(utilisationRate)) return 0;
            utilisationRate = utilisationRate / resources.Count();
            return utilisationRate * 100;
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
            var resources = db.Resources.ToList();

            /// Frequency rate: percentage of time space is used compared to its availability
            float frequencyRate = 0;

            foreach (DataLayer.Models.Resource data in resources)
            {
                var resource   = converter.ConvertDataResourceToWrapper(data);
                frequencyRate += CalculateResourceFrequencyRate(startDate, endDate, resource);
            }
            if (float.IsNaN(frequencyRate)) return 0;
            frequencyRate = frequencyRate / resources.Count();
            return frequencyRate * 100;
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
            var resources = db.Resources.ToList();

            /// Occupancy rate: how full the space is compared to its capacity
            float occupancyRate = 0;

            foreach (DataLayer.Models.Resource data in resources)
            {
                var resource   = converter.ConvertDataResourceToWrapper(data);
                occupancyRate += CalculateResourceOccupancyRate(startDate, endDate, resource);
            }
            if (float.IsNaN(occupancyRate)) return 0;
            occupancyRate = occupancyRate / resources.Count();
            return occupancyRate * 100;
        }


    }
}
