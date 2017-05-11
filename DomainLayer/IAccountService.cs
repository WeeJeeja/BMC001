using DomainLayer.WrapperModels;
using System;
namespace DomainLayer
{
    public interface IAccountService
    {
        /// <summary>
        /// Gets the user using the id
        /// </summary>
        /// <returns>Returns the user</returns>
        User GetUser(String email);

        /// <summary>
        /// Checks the email is not already registered in the database
        /// </summary>
        /// <param name="email">The email</param>
        /// <returns>True or false</returns>
        bool CheckEmailIsUnique(String email);

        /// <summary>
        /// Creates a new user and account
        /// </summary>
        /// <param name="user">The user</param>
        /// <param name="password">The account</param>
        void AddUserAndAccount(User user, string password);

        /// <summary>
        /// Checks the email and password match for a user in the system
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="password">The password</param>
        /// <returns>True or false</returns>
        bool Login(String email, String password);
    }
}
