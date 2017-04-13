using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IUserService
    {
        /// <summary>
        /// Gets all of the users from the database
        /// </summary>
        /// <returns>A list of users</returns>
        List<User> GetUsers();

        /// Gets the user using the id
        /// </summary>
        /// <returns>the user</returns>
        User GetUser(Guid? userId);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="data">The new user details</param>
        void UpdateUser(User data);

        /// <summary>
        /// Deletes an existing user from the database
        /// </summary>
        /// <param name="data">The user to be deleted</param>
        void DeleteUser(Guid? userId);
    }
}