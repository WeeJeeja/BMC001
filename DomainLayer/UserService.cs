using DataLayer;
using DomainLayer.WrapperModels;
using HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer
{
    public class UserService : IUserService
    {
        #region Fields

        ModelConversitions converter = new ModelConversitions();

        #endregion

        /// <summary>
        /// Gets all of the users in the database
        /// </summary>
        /// <returns>Returns a list of users</returns>
        public List<User> GetUsers()
        {
            var db = new ReScrumEntities();

            var userData = db.Users.Where(u => u.CancellationDate == null).ToList();
            var users    = new List<User>();

            foreach (DataLayer.Models.User data in userData)
            {
                var user = converter.ConvertDataUserToWrapper(data);

                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Gets the user using the id
        /// </summary>
        /// <returns>Returns the user</returns>
        public User GetUser(Guid? userId)
        {
            var db = new ReScrumEntities();

            var data = db.Users.Where(u => u.UserId == userId).FirstOrDefault();

            var user = converter.ConvertDataUserToWrapper(data);

            return user;
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="data">The new user details</param>
        public void UpdateUser(User data)
        {
            var db = new ReScrumEntities();

            var user = db.Users.Where(u => u.UserId == data.UserId).FirstOrDefault();

                user.EmployeeNumber   = data.EmployeeNumber;
                user.Forename         = data.Forename;
                user.Surname          = data.Surname;
                user.Email            = data.Email;
                user.JobTitle         = data.JobTitle;
                user.IsLineManager    = data.IsLineManager;
                user.IsAdministrator  = data.IsAdministrator;
                user.CancellationDate = data.CancellationDate;

            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing user from the database
        /// </summary>
        /// <param name="data">The user to be deleted</param>
        public void DeleteUser(Guid? userId)
        {
            var db = new ReScrumEntities();

            var user = db.Users.Where(u => u.UserId == userId).FirstOrDefault();

            user.CancellationDate = DateTime.Today;

            var teams = user.Teams.ToList();

            foreach (DataLayer.Models.Team team in teams)
            {
                team.Members.Remove(user);
            }

            db.SaveChanges();
        }
    }
}
