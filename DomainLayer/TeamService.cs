using DataLayer;
using DomainLayer.WrapperModels;
using HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class TeamService : ITeamService
    {
        #region Fields

        ModelConversitions converter = new ModelConversitions();

        #endregion
        /// <summary>
        /// Gets all of the teams in the database
        /// </summary>
        /// <returns>Returns a list of teams</returns>
        public List<Team> GetTeams()
        {
            var db = new ReScrumEntities();

            var data = db.Teams.ToList();
            var teams = new List<Team>();

            foreach (DataLayer.Models.Team t in data)
            {
                
                var team = new Team
                {
                    TeamId = t.TeamId,
                    Name = t.Name,
                    Colour = t.Colour,
                    Members = converter.ConvertDataUserListToWrapper(t.Members),
                };
                teams.Add(team);
            }


            return teams;
        }

        /// <summary>
        /// Gets the team using the id
        /// </summary>
        /// <returns>Returns the team</returns>
        public Team GetTeam(Guid? teamId)
        {
            var db = new ReScrumEntities();

            var data = db.Teams.Where(u => u.TeamId == teamId).FirstOrDefault();

            var team = new Team
            {

                TeamId          = data.TeamId,
                Name            = data.Name,
                Colour          = data.Colour,
                Members         = converter.ConvertDataUserListToWrapper(data.Members),
            };
            return team;
        }

        /// <summary>
        /// Adds a team to the database
        /// </summary>
        /// <param name="team">The new team to be added</param>
        public void AddTeam(Team team)
        {
            var db = new ReScrumEntities();

            var newTeam = new DataLayer.Models.Team
            {
                TeamId  = team.TeamId,
                Name    = team.Name,
                Colour  = team.Colour,
            };

            db.Teams.Add(newTeam);

            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing team
        /// </summary>
        /// <param name="data">The new team details</param>
        public void UpdateTeam(Team data)
        {
            var db = new ReScrumEntities();

            var team = db.Teams.Where(u => u.TeamId == data.TeamId).FirstOrDefault();

            team.TeamId  = data.TeamId;
            team.Name    = data.Name;
            team.Colour  = data.Colour;

            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing team from the database
        /// </summary>
        /// <param name="data">The team to be deleted</param>
        public void DeleteTeam(Guid? teamId)
        {
            var db = new ReScrumEntities();

            var team = db.Teams.Where(u => u.TeamId == teamId).FirstOrDefault();

            db.Teams.Remove(team);

            db.SaveChanges();
        }
    }
}
