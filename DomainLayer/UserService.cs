using DataLayer;
using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Gets all of the users in the database
        /// </summary>
        /// <returns>Returns a list of users</returns>
        public List<User> GetUsers()
        {
            var db = new ReScrumEntities();

            var data = db.Users.ToList();
            var users = new List<User>();

            foreach (DataLayer.Models.User u in data)
            {
                var user = new User
                {
                    UserId          = u.UserId,
                    EmployeeNumber  = u.EmployeeNumber,
                    Forename        = u.Forename,
                    Surname         = u.Surname,
                    JobTitle        = u.JobTitle,
                    IsLineManager   = u.IsLineManager,
                    IsAdministrator = u.IsAdministrator,

                };
                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Gets the user using the id
        /// </summary>
        /// <returns>Returns the user</returns>
        public User GetUser(Guid userId)
        {
            var db = new ReScrumEntities();

            var data = db.Users.Where(u => u.UserId == userId).FirstOrDefault();

            var user = new User
            {
                UserId          = data.UserId,
                EmployeeNumber  = data.EmployeeNumber,
                Forename        = data.Forename,
                Surname         = data.Surname,
                JobTitle        = data.JobTitle,
                IsLineManager   = data.IsLineManager,
                IsAdministrator = data.IsAdministrator,
            };


            return user;
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">The new user to be added</param>
        public void AddUser(User user)
        {
            var db = new ReScrumEntities();

            var newUser = new DataLayer.Models.User
            {
                EmployeeNumber  = user.EmployeeNumber,
                Forename        = user.Forename,
                Surname         = user.Surname,
                JobTitle        = user.JobTitle,
                IsLineManager   = user.IsLineManager,
                IsAdministrator = user.IsAdministrator,
            };

            db.Users.Add(newUser);

            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="data">The new user details</param>
        public void UpdateUser(User data)
        {
            var db = new ReScrumEntities();

            var user = db.Users.Where(u => u.UserId == data.UserId).FirstOrDefault();

                user.EmployeeNumber  = data.EmployeeNumber;
                user.Forename        = data.Forename;
                user.Surname         = data.Surname;
                user.JobTitle        = data.JobTitle;
                user.IsLineManager   = data.IsLineManager;
                user.IsAdministrator = data.IsAdministrator;

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

            db.Users.Remove(user);

            db.SaveChanges();
        }
    }
}
