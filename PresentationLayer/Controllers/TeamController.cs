using DomainLayer;
using PresentationLayer.HelperMethods;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class TeamController : Controller
    {
        #region Fields

        ITeamService service = new TeamService();
        IUserService userService = new UserService();
        ModelConversitions converter = new ModelConversitions();

        #endregion
        
        /// <summary>
        /// Team index displays a list of the current active teams
        /// </summary>
        /// <param name="successMessage">The success message</param>
        /// <returns>Team/Index</returns>
        public ActionResult Index(string successMessage = null)
        {
            ViewBag.Message = successMessage;

            //gets all of the teams from the database
            var data = service.GetTeams();

            var teams = converter.ConvertTeamsFromWrapper(data);

            return View(teams);
        }

        /// <summary>
        /// Displays the team details
        /// </summary>
        /// <param name="teamId">The team Id</param>
        /// <returns>Team/Details/TeamId</returns>
        public ActionResult Details(Guid? teamId)
        {
            var data = service.GetTeam(teamId);

            var team = converter.ConvertTeamFromWrapper(data);

            return View(team);
        }

        /// <summary>
        /// Gets the create page
        /// </summary>
        /// <returns>Team/Create</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new team
        /// </summary>
        /// <param name="collection">The team form data</param>
        /// <returns>Team/Index</returns>
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var team = new DomainLayer.WrapperModels.Team();
                UpdateModel(team, collection);

                service.AddTeam(team);

                return RedirectToAction("Index", new { successMessage = team.Name + " added successfully." });
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets the edit team page
        /// </summary>
        /// <param name="teamId">The team id</param>
        /// <returns>Team/Edit/TeamId</returns>
        public ActionResult Edit(Guid? teamId)
        {
            var data = service.GetTeam(teamId);

            var team = new Team
            {
                TeamId = data.TeamId,
                Name = data.Name,
                Colour = data.Colour,
            };

            return View(team);


        }

        /// <summary>
        /// Posts the team update
        /// </summary>
        /// <param name="teamId">The team</param>
        /// <param name="collection">The team update data</param>
        /// <returns>Team/Update</returns>
        [HttpPost]
        public ActionResult Edit(Guid? teamId, FormCollection collection)
        {
            try
            {
                var team = new DomainLayer.WrapperModels.Team();
                UpdateModel(team, collection);

                service.UpdateTeam(team);

                return RedirectToAction("Index", new { successMessage = team.Name + "updated successfully!"});
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets the add team memeber page
        /// </summary>
        /// <param name="teamId">The team id</param>
        /// <returns>Team/AddMember/TeamId</returns>
        public ActionResult AddMember(Guid? teamId)
        {
            var data = service.GetTeam(teamId);

            var team = new Team
            {
                TeamId = data.TeamId,
                Name   = data.Name,
                Colour = data.Colour,
            };

            var nonMembers = service.GetPotentialTeamMembers(teamId);

            team.PotentialMembers = converter.ConvertUserListFromWrapper(nonMembers);

            return View(team);
        }

        /// <summary>
        /// Adds a member to a team
        /// </summary>
        /// <param name="teamId">The team</param>
        /// <param name="userId">The user</param>
        /// <returns>Team/Index</returns>
        [HttpPost]
        public ActionResult AddMember(Guid? teamId, Guid? userId)
        {
            try
            {
                service.AddMember(teamId, userId);

                var user = userService.GetUser(userId);
                var team = service.GetTeam(teamId);

                var successMessage = user.Forename + " " + user.Surname + " was successfully added to team " + team.Name;

                return RedirectToAction("Index", new { successMessage = successMessage });
            }
            catch
            {
                return View();
            }
        }


        /// <summary>
        /// Gets the delete page
        /// </summary>
        /// <param name="teamId">The team</param>
        /// <returns>Team/Delete</returns>
        public ActionResult Delete(Guid? teamId)
        {
            var data = service.GetTeam(teamId);

            var team = new Team
            {
                TeamId = data.TeamId,
                Name   = data.Name,
                Colour = data.Colour,
            };

            return View(team);
        }

        /// <summary>
        /// Deletes a team
        /// </summary>
        /// <param name="teamId">The team</param>
        /// <param name="collection">form data</param>
        /// <returns>Team/Index</returns>
        [HttpPost]
        public ActionResult Delete(Guid? teamId, FormCollection collection)
        {
            try
            {
                service.DeleteTeam(teamId);

                return RedirectToAction("Index", new { successMessage = "Team successfully deleted"});
            }
            catch
            {
                return View();
            }
        }
    }
}
