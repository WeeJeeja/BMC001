using DataLayer;
using DomainLayer.WrapperModels;
using data = DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class AccountService : IAccountService
    {
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
                EmployeeNumber = data.EmployeeNumber,
                Forename = data.Forename,
                Surname = data.Surname,
                JobTitle = data.JobTitle,
                IsLineManager = data.IsLineManager,
                IsAdministrator = data.IsAdministrator,
            };

            return user;
        }

        public bool CheckEmailIsUnique(String email)
        {
            var db = new ReScrumEntities();

            if (db.Users.Where(u => u.Email.Equals(email)).Any()) return false;

            return true;
        }

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

            var account = new data.Account
            {
                User     = newUser,
                Password = Encoding.UTF8.GetBytes(password),

            };
            db.Accounts.Add(account);

            db.SaveChanges();
        }

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
