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

        //Seed method to populate data into the database on intial creation
        protected override void Seed(DataLayer.ReScrumEntities context)
        {
            #region Adding slots

            context.Slots.AddOrUpdate(s => s.SlotId,
                new Slot { Time = "09:00 - 10:00", StartTime = TimeSpan.FromHours(9),  EndTime = TimeSpan.FromHours(10) },
                new Slot { Time = "10:00 - 11:00", StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(11) },
                new Slot { Time = "11:00 - 12:00", StartTime = TimeSpan.FromHours(11), EndTime = TimeSpan.FromHours(12) },
                new Slot { Time = "12:00 - 13:00", StartTime = TimeSpan.FromHours(12), EndTime = TimeSpan.FromHours(13) },
                new Slot { Time = "13:00 - 14:00", StartTime = TimeSpan.FromHours(13), EndTime = TimeSpan.FromHours(14) },
                new Slot { Time = "14:00 - 15:00", StartTime = TimeSpan.FromHours(14), EndTime = TimeSpan.FromHours(15) },
                new Slot { Time = "15:00 - 16:00", StartTime = TimeSpan.FromHours(15), EndTime = TimeSpan.FromHours(16) },
                new Slot { Time = "16:00 - 17:00", StartTime = TimeSpan.FromHours(16), EndTime = TimeSpan.FromHours(17) });

            #endregion

            #region Adding users

            ////Add user1 to the databse first so that they can be used as the line manager for all other users
            //var user1 = new User { EmployeeNumber = 10000001, Forename = "Saoirse", Surname = "Kelly", JobTitle = "Graduate Software Engineer", IsLineManager = true, IsAdministrator = true };
            //context.Users.Add(user1);

            //context.Users.AddOrUpdate(u   => u.UserId,
            //    new User { EmployeeNumber = 10000002, Forename = "Caoimhe", Surname = "Bell", JobTitle = "Receptionist", IsLineManager = false, IsAdministrator = false, LineManager = user1 },
            //    new User { EmployeeNumber = 10000003, Forename = "Clodagh", Surname = "McCann", JobTitle = "Placement BA", IsLineManager = false, IsAdministrator = false, LineManager = user1 },
            //    new User { EmployeeNumber = 10000004, Forename = "Anthony", Surname = "McCann", JobTitle = "Placement software engineer", IsLineManager = false, IsAdministrator = false, LineManager = user1 },
            //    new User { EmployeeNumber = 10000005, Forename = "James", Surname = "Murphy", JobTitle = "Senior  software engineer", IsLineManager = false, IsAdministrator = false, LineManager = user1 }
            //    );

            //context.SaveChanges();

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

            ////Adds every user in the database to the team
            //foreach (User user in context.Users)
            //{
            //    context.Teams.Where(t => t.Name.Equals("BMC001")).FirstOrDefault().Members.Add(user);
            //}
            context.SaveChanges();

            #endregion

            #region Adding resources

            context.Resources.AddOrUpdate(r => r.ResourceId,
                new Resource
                {
                    Name = "Desk A1",
                    Description = "PC and phone",
                    Category = "Desk",
                    Capacity = 1,
                    Location = "x, y",
                });

            context.Resources.AddOrUpdate(r => r.ResourceId,
                new Resource
                {
                    Name = "Spindle",
                    Description = "Large table, 6 chairs and projector",
                    Category = "Small meeting room",
                    Capacity = 6,
                    Location = "x, y",
                });

            context.Resources.AddOrUpdate(r => r.ResourceId,
                new Resource
                {
                    Name = "Desk B2",
                    Description = "Empty desk",
                    Category = "Desk",
                    Capacity = 1,
                    Location = "x, y",
                });

            context.Resources.AddOrUpdate(r => r.ResourceId,
                new Resource
                {
                    Name = "CEO Office",
                    Description = "PC, phone & printer",
                    Category = "Private office",
                    Capacity = 1,
                    Location = "x, y",
                });

            context.SaveChanges();

            #endregion

        }
    }
}
