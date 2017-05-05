using NUnit.Framework;
using PresentationLayer.Controllers;
using PresentationLayer.HelperMethods;
using PresentationLayer.Models;
using wrapper = DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;

namespace PresentationLayer.Tests.HelperMethods
{
    [TestFixture]
    public class ModelConversionsTests
    {
        #region Convert to view models .Tests

        [Test]
        public void Convert_Slot_From_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();
            var model = new wrapper.Slot
            {
                SlotId = new Guid(),
                Time = "09:00",
            };

            //Act
            var result = converter.ConvertSlotFromWrapper(model);

            //Assert
            Assert.IsAssignableFrom<Slot>(result);
            Assert.AreEqual(result.SlotId, model.SlotId);
            Assert.AreEqual(result.Time, model.Time);
        }

        [Test]
        public void Convert_Team_From_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();
            var model = new wrapper.Team
            {
                TeamId  = new Guid(),
                Name    = "BMC001",
                Colour  = "Yellow",
                Members = new List<wrapper.User>(),
            };

            model.Members.Add(new wrapper.User
                {
                    UserId   = new Guid(),
                    Forename = "Jamie",
                    Surname  = "Matthews",
                    JobTitle = "Placement Engineer",
                });

            model.Members.Add(new wrapper.User
            {
                UserId   = new Guid(),
                Forename = "Sarah",
                Surname  = "Jones",
                JobTitle = "Senior Engineer",
            });

            //Act
            var result = converter.ConvertTeamFromWrapper(model);

            //Assert
            Assert.IsAssignableFrom<Team>(result);
            Assert.AreEqual(2, result.Members.Count);
            Assert.AreEqual(result.TeamId, model.TeamId);
            Assert.AreEqual(result.Name, model.Name);
            Assert.AreEqual(result.Colour, model.Colour);
            Assert.IsAssignableFrom<List<User>>(result.Members);
        }

        [Test]
        public void Convert_Team_List_From_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();

            var model = new List<wrapper.Team>();

            model.Add(new wrapper.Team
            {
                TeamId = new Guid(),
                Name = "BMC001",
                Colour = "Yellow",
            });

            model.Add(new wrapper.Team
            {
                TeamId = new Guid(),
                Name = "BMC008",
                Colour = "Blue",
            });

            //Act
            var result = converter.ConvertTeamsFromWrapper(model);

            //Assert
            Assert.IsAssignableFrom<List<Team>>(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Convert_User_From_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();

            var model = new wrapper.User
            {
                UserId          = new Guid(),
                Forename        = "Jamie",
                Surname         = "Matthews",
                JobTitle        = "Placement Engineer",
                IsAdministrator = false,
                IsLineManager   = true,
                Email           = "jamie@hotmail.com"
            };

            //Act
            var result = converter.ConvertUserFromWrapper(model);

            //Assert
            Assert.IsAssignableFrom<User>(result);
            Assert.AreEqual(result.UserId, model.UserId);
            Assert.AreEqual(result.Forename, model.Forename);
            Assert.AreEqual(result.Surname, model.Surname);
            Assert.AreEqual(result.JobTitle, model.JobTitle);
            Assert.AreEqual(result.IsAdministrator, model.IsAdministrator);
            Assert.AreEqual(result.IsLineManager, model.IsLineManager);
            Assert.AreEqual(result.Email, model.Email);
        }

        [Test]
        public void Convert_User_List_From_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();

            var model = new List<wrapper.User>();

            model.Add(new wrapper.User
            {
                UserId          = new Guid(),
                Forename        = "Jamie",
                Surname         = "Matthews",
                JobTitle        = "Placement Engineer",
                IsAdministrator = false,
                IsLineManager   = true,
                Email           = "jamie@hotmail.com"
            });

            model.Add(new wrapper.User
            {
                UserId          = new Guid(),
                Forename        = "Jamie",
                Surname         = "Matthews",
                JobTitle        = "Placement Engineer",
                IsAdministrator = false,
                IsLineManager   = true,
                Email           = "jamie@hotmail.com"
            });

            //Act
            var result = converter.ConvertUserListFromWrapper(model);

            //Assert
            Assert.IsAssignableFrom<List<User>>(result);
            Assert.AreEqual(2, model.Count);
        }

        [Test]
        public void Convert_Resource_From_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();

            var model = new wrapper.Resource
            {
                ResourceId  = new Guid(),
                Name        = "Desk A1",
                Description = "Hot Desk",
                Location    = "x, y",
                Capacity    = 1,
                Category    = "Desk",
            };

            //Act
            var result = converter.ConvertResourceFromWrapper(model);

            //Assert
            Assert.IsAssignableFrom<Resource>(result);
            Assert.AreEqual(result.ResourceId, model.ResourceId);
            Assert.AreEqual(result.Name, model.Name);
            Assert.AreEqual(result.Description, model.Description);
            Assert.AreEqual(result.Location, model.Location);
            Assert.AreEqual(result.Capacity, model.Capacity);
            Assert.AreEqual(result.Category, model.Category);
        }

        [Test]
        public void Convert_Resource_List_From_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();

            var model = new List<wrapper.Resource>();

            model.Add(new wrapper.Resource
                {
                    ResourceId = new Guid(),
                    Name = "Desk A1",
                    Description = "Hot Desk",
                    Location = "x, y",
                    Capacity = 1,
                    Category = "Desk",
                });

            model.Add(new wrapper.Resource
                {
                    ResourceId = new Guid(),
                    Name = "Spindle",
                    Description = "Conference room",
                    Location = "x, y",
                    Capacity = 200,
                    Category = "Room",
                });

            //Act
            var result = converter.ConvertResourceListFromWrapper(model);

            //Assert
            Assert.IsAssignableFrom<List<Resource>>(result);
            Assert.AreEqual(2, model.Count);
        }

        #endregion

        #region Convert to wrappers .Tests

        [Test]
        public void Convert_Slot_To_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();
            var model = new Slot
            {
                SlotId = new Guid(),
                Time = "09:00",
            };

            //Act
            var result = converter.ConvertSlotToWrapper(model);

            //Assert
            Assert.IsAssignableFrom<wrapper.Slot>(result);
            Assert.AreEqual(result.SlotId, model.SlotId);
            Assert.AreEqual(result.Time, model.Time);
        }

        [Test]
        public void Convert_User_To_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();

            var model = new User
            {
                UserId = new Guid(),
                Forename = "Jamie",
                Surname = "Matthews",
                JobTitle = "Placement Engineer",
                IsAdministrator = false,
                IsLineManager = true,
                Email = "jamie@hotmail.com"
            };

            //Act
            var result = converter.ConvertUserToWrapper(model);

            //Assert
            Assert.IsAssignableFrom<wrapper.User>(result);
            Assert.AreEqual(result.UserId, model.UserId);
            Assert.AreEqual(result.Forename, model.Forename);
            Assert.AreEqual(result.Surname, model.Surname);
            Assert.AreEqual(result.JobTitle, model.JobTitle);
            Assert.AreEqual(result.IsAdministrator, model.IsAdministrator);
            Assert.AreEqual(result.IsLineManager, model.IsLineManager);
            Assert.AreEqual(result.Email, model.Email);
        }

        [Test]
        public void Convert_Resource_To_Wrapper_To_View_Model()
        {
            //Arrange
            var converter = new ModelConversitions();

            var model = new Resource
            {
                ResourceId  = new Guid(),
                Name        = "Desk A1",
                Description = "Hot Desk",
                Location    = "x, y",
                Capacity    = 1,
                Category    = "Desk",
            };

            //Act
            var result = converter.ConvertResourceToWrapper(model);

            //Assert
            Assert.IsAssignableFrom<wrapper.Resource>(result);
            Assert.AreEqual(result.ResourceId, model.ResourceId);
            Assert.AreEqual(result.Name, model.Name);
            Assert.AreEqual(result.Description, model.Description);
            Assert.AreEqual(result.Location, model.Location);
            Assert.AreEqual(result.Capacity, model.Capacity);
            Assert.AreEqual(result.Category, model.Category);
        }

        #endregion
    }
}
