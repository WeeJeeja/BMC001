﻿using NUnit.Framework;
using PresentationLayer.Models;
using System;

namespace PresentationLayer.Tests.Models
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void User_Employee_Number_Tests()
        {
            var model = new User()
            {
                EmployeeNumber = 10000001,
                Forename       = "Saoirse",
                Surname        = "McCann",
                Email          = "smccann60@qub.ac.uk",
                JobTitle       = "Placement Software Engineer",
            };

            var results = model;

            Assert.AreEqual(10000001, results.EmployeeNumber);
        }

        [Test]
        public void User_Forename_Tests()
        {
            var model = new User()
            {
                EmployeeNumber = 10000001,
                Forename = "Saoirse",
                Surname  = "McCann",
                Email    = "smccann60@qub.ac.uk",
                JobTitle = "Placement Software Engineer",
            };

            var results = model;

            Assert.AreEqual("Saoirse", results.Forename);
        }

        [Test]
        public void User_Surname_Tests()
        {
            var model = new User()
            {
                EmployeeNumber = 10000001,
                Forename = "Saoirse",
                Surname = "McCann",
                Email = "smccann60@qub.ac.uk",
                JobTitle = "Placement Software Engineer",
            };

            var results = model;

            Assert.AreEqual("McCann", results.Surname);
        }

        [Test]
        public void User_Email_Tests()
        {
            var model = new User()
            {
                EmployeeNumber = 10000001,
                Forename = "Saoirse",
                Surname = "McCann",
                Email = "smccann60@qub.ac.uk",
                JobTitle = "Placement Software Engineer",
            };

            var results = model;

            Assert.AreEqual("smccann60@qub.ac.uk", results.Email);
        }
    }
}
