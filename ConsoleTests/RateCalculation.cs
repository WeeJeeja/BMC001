using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    public class RateCalculation
    {
           static Slot slot1 = new Slot { Time = "09:00 - 10:00" };
           static Slot slot2 = new Slot { Time = "10:00 - 11:00" };
           static Slot slot3 = new Slot { Time = "11:00 - 12:00" };
           static Slot slot4 = new Slot { Time = "12:00 - 13:00" };
           static Slot slot5 = new Slot { Time = "13:00 - 14:00" };
           static Slot slot6 = new Slot { Time = "14:00 - 15:00" };
           static Slot slot7 = new Slot { Time = "15:00 - 16:00" };
           static Slot slot8 = new Slot { Time = "16:00 - 17:00" };
           static Slot slot9 = new Slot { Time = "17:00 - 18:00" };

            static List<Slot> slots = new List<Slot>();

            static Resource desk1 = new Resource
            {
                ResourceId = Guid.NewGuid(),
                Name = "Desk 1",
                Description = "PC and phone",
                Category = "Desk",
                Capacity = 1,
                Location = "x, y",
            };

            static Resource room1 = new Resource
            {
                ResourceId = Guid.NewGuid(),
                Name = "Room 1",
                Description = "Conference room with projector",
                Category = "Room",
                Capacity = 200,
                Location = "x, y",
            };

            static User user = new User
            {
                UserId = Guid.NewGuid(),
                EmployeeNumber = 10000001, 
                Forename = "Saoirse", 
                Surname = "Kelly", JobTitle = "Software engineer", 
                IsLineManager = true, 
                IsAdministrator = true
            };

            static List<Booking> schedule = new List<Booking>();

            
        //static void Main(string[] args)
        //{
        //    slots.Add(slot1);
        //    slots.Add(slot2);
        //    slots.Add(slot3);
        //    slots.Add(slot4);
        //    slots.Add(slot5);
        //    slots.Add(slot6);
        //    slots.Add(slot7);
        //    slots.Add(slot8);
        //    slots.Add(slot9);

        //    schedule.Add(new Timetable 
        //        {
        //            TimetableId = Guid.NewGuid(),
        //            Date = new DateTime(2016, 12, 5),
        //            Slot = slots.Where(s => s.Time.Equals("9:00 - 10:00")).FirstOrDefault(),
        //            User = user,
        //            Resource = desk1,
        //            Capacity = 1
        //        });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("10:00 - 11:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = desk1,
        //        Capacity = 1
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("11:00 - 12:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = desk1,
        //        Capacity = 1
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("9:00 - 10:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("10:00 - 11:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("11:00 - 12:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("12:00 - 13:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("13:00 - 14:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("14:00 - 15:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("15:00 - 16:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("16:00 - 17:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });

        //    schedule.Add(new Timetable
        //    {
        //        TimetableId = Guid.NewGuid(),
        //        Date = new DateTime(2016, 12, 5),
        //        Slot = slots.Where(s => s.Time.Equals("17:00 - 18:00")).FirstOrDefault(),
        //        User = user,
        //        Resource = room1,
        //        Capacity = 100
        //    });


        //    #region Frequency rate

        //    //Console.WriteLine("---------- Rates for Desk 1 ---------");
        //    //CalculateResourceFrequencyRate(new DateTime(2016, 12, 5), new DateTime(2016, 12, 5), desk1);

        //    //Console.WriteLine("---------- Rates for Room 1 ---------");
        //    //CalculateResourceFrequencyRate(new DateTime(2016, 12, 5), new DateTime(2016, 12, 5), room1);

        //    #endregion

        //    #region Occupancy rate

        //    //Console.WriteLine("---------- Rates for Desk 1 ---------");
        //    //CalculateResourceOccupancyRate(new DateTime(2016, 12, 5), new DateTime(2016, 12, 5), desk1);

        //    //Console.WriteLine("---------- Rates for Room 1 ---------");
        //    //CalculateResourceOccupancyRate(new DateTime(2016, 12, 5), new DateTime(2016, 12, 5), room1);

        //    #endregion

        //    #region Utilisation rate

        //    Console.WriteLine("*---------- Rates for Desk 1 ---------*");
        //    CalculateResourceUtilisationRate(new DateTime(2016, 12, 5), new DateTime(2016, 12, 5), desk1);

        //    Console.WriteLine("*---------- Rates for Room 1 ---------*");
        //    CalculateResourceUtilisationRate(new DateTime(2016, 12, 5), new DateTime(2016, 12, 5), room1);

        //    Console.ReadKey();

        //    #endregion
        //}

        /// <summary>
        /// Calculates the number of slots (hours) the resource was in use
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource</param>
        /// <returns>The number of slots the resource was booked for during the date range entered</returns>
        static public List<Booking> GetSlotsUtilised(DateTime startDate, DateTime endDate, Resource resource)
        {
            return schedule.Where(r => r.Resource.Equals(resource) &&
                                            r.Date >= startDate &&
                                            r.Date <= endDate).ToList();
        }

        /// <summary>
        /// Calculates the frquency rate for an individual resource between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the frequency rate for</param>
        /// <returns>The frequency rate for the resource</returns>
        static public float CalculateResourceFrequencyRate(DateTime startDate, DateTime endDate, Resource resource)
        {
            //Frequency rate: percentage of time space is used compared to its availability
            Console.WriteLine("------ Frequency rate for " + resource.Name + " -----");

            //Calculate how many days
            var dateRange = endDate.DayOfYear - startDate.DayOfYear + 1;

            Console.WriteLine("Frequency rate calculation between " + startDate.ToShortDateString() + " and " + endDate.ToShortDateString() + " - " + dateRange + " days in total.");

            //Resource availability
            Console.WriteLine("There are " + slots.Count + " hours available each day");

            var availability = slots.Count() * dateRange;
            Console.WriteLine("The resource availability for " + dateRange + " days is: " + availability + " hours");

            //How many times the resource was used
            var hoursUsed = GetSlotsUtilised(startDate, endDate, resource).Count();
            Console.WriteLine("The resource has been used for " + hoursUsed + " hour(s) during said time period.");


            //Number of slots the space was in use/Number of slots available = frequency rate %
            var frequencyRate = ((float)hoursUsed / (float)availability);

            Console.WriteLine("The frequency rate for " + resource.Name + " is: " + frequencyRate * 100 + "%");

            return frequencyRate;
        }

        /// <summary>
        /// Calculates the occupnacy rate for an individual resource between a given date range
        /// </summary>
        /// <param name="startDate">The start date to search from</param>
        /// <param name="endDate">The end date to search to</param>
        /// <param name="resource">The resource to calculate the occupancy rate for</param>
        /// <returns>The occupancy rate for the resource</returns>
        static public float CalculateResourceOccupancyRate(DateTime startDate, DateTime endDate, Resource resource)
        {
            //Occupancy rate: how full the space is compared to its capacity
            Console.WriteLine("------ Occupancy rate for " + resource.Name + " -----");

            //Calculate how many days
            var dateRange = endDate.DayOfYear - startDate.DayOfYear + 1;

            Console.WriteLine("Occupancy rate calculation between " + startDate.ToShortDateString() + " and " + endDate.ToShortDateString() + " - " + dateRange + " days in total.");

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
        static public float CalculateResourceUtilisationRate(DateTime startDate, DateTime endDate, Resource resource)
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
