using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface ITeamService
    {
        void AddTeam(Team team);
        void DeleteTeam(Guid? teamId);
        Team GetTeam(Guid? teamId);
        List<Team> GetTeams();

        void UpdateTeam(Team data);

        /// <summary>
        /// Adds a user to the team
        /// </summary>
        /// <param name="data">The teamId and the new userId</param>
        void AddMember(Guid? teamId, Guid? userId);

        List<User> GetTeamMembers(Guid? teamId);

        /// <summary>
        /// Gets all of the users that are not a member of the team
        /// </summary>
        /// <returns>Returns a list of users</returns>
        List<User> GetPotentialTeamMembers(Guid? teamId);
    }
}
