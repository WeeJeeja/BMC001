using DataLayer;
using DomainLayer.WrapperModels;
using data = DataLayer.Models;
using System;
using System.Linq;
using System.Text;

namespace DomainLayer
{
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Gets the user using the id
        /// </summary>
        /// <returns>Returns the user</returns>
        public User GetUser(String email)
        {
            var db = new ReScrumEntities();

            var data = db.Users.Where(u => u.Email == email).FirstOrDefault();

            var user = new User
            {
                UserId          = data.UserId,
                EmployeeNumber  = data.EmployeeNumber,
                Forename        = data.Forename,
                Surname         = data.Surname,
                Email           = data.Email,
                JobTitle        = data.JobTitle,
                IsLineManager   = data.IsLineManager,
                IsAdministrator = data.IsAdministrator,
            };

            return user;
        }

        /// <summary>
        /// Checks the email is not already registered in the database
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns>True or false</returns>
        public bool CheckEmailIsUnique(String email)
        {
            var db = new ReScrumEntities();

            if (db.Users.Where(u => u.Email.Equals(email)).Any()) return false;

            return true;
        }

        /// <summary>
        /// Creates a new user and account
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="password">The account</param>
        public void AddUserAndAccount(User user, string password)
        {
            var db = new ReScrumEntities();

            var newUser = new data.User
            {
                EmployeeNumber  = user.EmployeeNumber,
                Forename        = user.Forename,
                Surname         = user.Surname,
                Email           = user.Email,
                JobTitle        = user.JobTitle,
                IsLineManager   = user.IsLineManager,
                IsAdministrator = user.IsAdministrator,
            };
            db.Users.Add(newUser);

            var team = db.Teams.Where(t => t.TeamId == user.Team).FirstOrDefault();
            newUser.Teams.Add(team);

            var account = new data.Account
            {
                User     = newUser,
                Password = Encoding.UTF8.GetBytes(password),
            };
            db.Accounts.Add(account);

            db.SaveChanges();
        }

        /// <summary>
        /// Checks the email and password match for a user in the system
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        /// <returns>True or false</returns>
        public bool Login(String email, String password)
        {
            var db = new ReScrumEntities();

            var user = db.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
            if (user == null) return false;

            var account = db.Accounts.Where(a => a.User.UserId == user.UserId).FirstOrDefault();
            if (account == null) return false;

            var savedPassword = Encoding.UTF8.GetString(account.Password);

            if (!savedPassword.Equals(password)) return false;

            return true;
        }
    }
}
