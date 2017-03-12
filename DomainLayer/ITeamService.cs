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
        List<User> GetTeamMembers(Guid? teamId);
    }
}
