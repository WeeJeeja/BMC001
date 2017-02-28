namespace DataLayer.Migrations
{
    using DataLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataLayer.ReScrumEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DataLayer.ReScrumEntities context)
        {
            #region Adding slots

            context.Slots.AddOrUpdate(s => s.SlotId,
                new Slot { Time = "09:00 - 10:00" },
                new Slot { Time = "10:00 - 11:00" },
                new Slot { Time = "11:00 - 12:00" },
                new Slot { Time = "12:00 - 13:00" },
                new Slot { Time = "13:00 - 14:00" },
                new Slot { Time = "14:00 - 15:00" },
                new Slot { Time = "15:00 - 16:00" },
                new Slot { Time = "16:00 - 17:00" },
                new Slot { Time = "17:00 - 18:00" });

            #endregion

            #region Adding users

            //Add user1 to the databse first so that they can be used as the line manager for all other users
            var user1 = new User { EmployeeNumber = 10000001, Forename = "Saoirse", Surname = "Kelly", JobTitle = "Software engineer", IsLineManager = true, IsAdministrator = true };
            context.Users.Add(user1);

            context.Users.AddOrUpdate(u   => u.UserId,
                new User { EmployeeNumber = 10000002, Forename = "Caoimhe", Surname = "Bell", JobTitle = "Receptionist", IsLineManager = false, IsAdministrator = false, LineManager = user1 },
                new User { EmployeeNumber = 10000003, Forename = "Clodagh", Surname = "McCann", JobTitle = "Placement BA", IsLineManager = false, IsAdministrator = false, LineManager = user1 },
                new User { EmployeeNumber = 10000004, Forename = "Anthony", Surname = "McCann", JobTitle = "Placement software engineer", IsLineManager = false, IsAdministrator = false, LineManager = user1 },
                new User { EmployeeNumber = 10000005, Forename = "James", Surname = "Murphy", JobTitle = "Senior  software engineer", IsLineManager = false, IsAdministrator = false, LineManager = user1 }
                );

            context.SaveChanges();

            #endregion

            #region Adding team

            //Creates the team BMC001
            context.Teams.AddOrUpdate(t => t.TeamId,
                new Team
                {
                    Name = "BMC001",
                    Colour = "Yellow",
                    Members = new List<User>(),
                });
            context.SaveChanges();

            //Adds every user in the database to the team
            foreach (User user in context.Users)
            {
                context.Teams.Where(t => t.Name.Equals("BMC001")).FirstOrDefault().Members.Add(user);
            }
            context.SaveChanges();

            #endregion

            #region Adding resources

            context.Resources.AddOrUpdate(r => r.ResourceId,
                new Resource
                {
                    Name = "Desk 1",
                    Description = "PC and phone",
                    Category = "Desk",
                    Capacity = 1,
                    Location = "x, y",
                });

            context.Resources.AddOrUpdate(r => r.ResourceId,
                new Resource
                {
                    Name = "Room 1",
                    Description = "Conference room with projector",
                    Category = "Room",
                    Capacity = 200,
                    Location = "x, y",
                });

            context.Resources.AddOrUpdate(r => r.ResourceId,
                new Resource
                {
                    Name = "Desk 2",
                    Description = "Empty desk",
                    Category = "Desk",
                    Capacity = 1,
                    Location = "x, y",
                });

            context.SaveChanges();

            #endregion

            //#region Adding bookings

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("09:00 - 10:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Desk 1")).FirstOrDefault(),
            //        Capacity = 1
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("10:00 - 11:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Desk 1")).FirstOrDefault(),
            //        Capacity = 1
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("11:00 - 12:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Desk 1")).FirstOrDefault(),
            //        Capacity = 1
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("09:00 - 10:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("10:00 - 11:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("11:00 - 12:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("12:00 - 13:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("13:00 - 14:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("14:00 - 15:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("15:00 - 16:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("16:00 - 17:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.Booking.AddOrUpdate(t => t.BookingId,
            //    new Booking
            //    {
            //        Date = new DateTime(2016, 12, 5),
            //        Slot = context.Slots.Where(s => s.Time.Equals("17:00 - 18:00")).FirstOrDefault(),
            //        User = user1,
            //        Resource = context.Resources.Where(s => s.Name.Equals("Room 1")).FirstOrDefault(),
            //        Capacity = 100
            //    });

            //context.SaveChanges();

            //#endregion
        }
    }
}
