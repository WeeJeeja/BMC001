using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IUserService
    {
        List<User> GetUsers();

        /// Gets the user using the id
        /// </summary>
        /// <returns>the user</returns>
        User GetUser(Guid? userId);

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">The new user to be added</param>
        void AddUser(User user);

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