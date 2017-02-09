using System;
namespace DomainLayer
{
    public interface ITeamService
    {
        void AddTeam(DomainLayer.WrapperModels.Team team);
        void DeleteTeam(Guid? teamId);
        DomainLayer.WrapperModels.Team GetTeam(Guid? teamId);
        System.Collections.Generic.List<DomainLayer.WrapperModels.Team> GetTeams();
        void UpdateTeam(DomainLayer.WrapperModels.Team data);
    }
}
