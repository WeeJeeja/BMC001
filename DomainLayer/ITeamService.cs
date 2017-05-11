using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface ITeamService
    {
        /// <summary>
        /// Gets all of the teams in the database
        /// </summary>
        /// <returns>Returns a list of teams</returns>
        List<Team> GetTeams();

        /// <summary>
        /// Gets the team using the id
        /// </summary>
        /// <returns>Returns the team</returns>
        Team GetTeam(Guid? teamId);

        /// <summary>
        /// Adds a team to the database
        /// </summary>
        /// <param name="team">The new team to be added</param>
        void AddTeam(Team team);

        /// <summary>
        /// Updates an existing team
        /// </summary>
        /// <param name="data">The new team details</param>
        void UpdateTeam(Team data);

        /// <summary>
        /// Adds a user to the team
        /// </summary>
        /// <param name="data">The teamId and the new userId</param>
        void AddMember(Guid? teamId, Guid? userId);

        /// <summary>
        /// Deletes an existing team from the database
        /// </summary>
        /// <param name="data">The team to be deleted</param>
        void DeleteTeam(Guid? teamId);

        /// <summary>
        /// Gets all of the members on a team
        /// </summary>
        /// <returns>Returns a list of users</returns>
        List<User> GetTeamMembers(Guid? teamId);

        /// <summary>
        /// Gets all of the users that are not a member of the team
        /// </summary>
        /// <returns>Returns a list of users</returns>
        List<User> GetPotentialTeamMembers(Guid? teamId);
    }
}
