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
    public class RateService : DomainLayer.IRateService
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

            var bookings = db.Booking.Where(r => r.Resource.Equals(resource) &&
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

            Console.WriteLine("------ Utilisation rate for " + resource.Name + " -----");
            var utilisationRate = frequencyRate * occupancyRate;
            Console.WriteLine("The utilisation rate for " + resource.Name + " is: " + utilisationRate * 100 + "%");
            return utilisationRate;
        }
    }
}
