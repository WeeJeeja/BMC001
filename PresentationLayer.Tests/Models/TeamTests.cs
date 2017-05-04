using NUnit.Framework;
using PresentationLayer.Models;
using System;

namespace PresentationLayer.Tests.Models
{
    [TestFixture]
    public class TeamTests
    {
        [Test]
        public void Team_Id_Test()
        {
            //Arrange
            var model = new Team()
            {
                TeamId = new Guid(),
                Name   = "BMC0001",
                Colour = "Yellow"
            };

            //Act
            var results = model;

            //Assert
            Assert.AreEqual(model.TeamId, results.TeamId);
        }

        [Test]
        public void Team_Name_Test()
        {
            //Arrange
            var model = new Team()
            {
                TeamId = new Guid(),
                Name = "BMC0001",
                Colour = "Yellow"
            };

            //Act
            var results = model;

            //Assert
            Assert.AreEqual("BMC0001", results.Name);
        }

        [Test]
        public void Team_Colour_Test()
        {
            //Arrange
            var model = new Team()
            {
                TeamId = new Guid(),
                Name = "BMC0001",
                Colour = "Yellow"
            };

            //Act
            var results = model;

            //Assert
            Assert.AreEqual("Yellow", results.Colour);
        }
    }
}
