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

        //
        // GET: /Team/

        public ActionResult Index(string successMessage = null)
        {
            ViewBag.Message = successMessage;

            var data = service.GetTeams();
            var teams = new List<Team>();

            foreach (DomainLayer.WrapperModels.Team t in data)
            {
                var team = new Team
                {
                    TeamId = t.TeamId,
                    Name = t.Name,
                    Colour = t.Colour,
                };
                teams.Add(team);
            }

            return View(teams);
        }

        //
        // GET: /Team/Details/5

        public ActionResult Details(Guid? teamId)
        {
            var data = service.GetTeam(teamId);

            var team = converter.ConvertTeamFromWrapper(data);

            return View(team);
        }

        //
        // GET: /Team/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Team/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var team = new DomainLayer.WrapperModels.Team();
                UpdateModel(team, collection);

                service.AddTeam(team);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Team/Edit/5

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

        //
        // POST: /Team/Edit/5

        [HttpPost]
        public ActionResult Edit(Guid? teamId, FormCollection collection)
        {
            try
            {
                var team = new DomainLayer.WrapperModels.Team();
                UpdateModel(team, collection);

                service.UpdateTeam(team);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Team/AddMember/5

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

        //
        // POST: /Team/AddMember/5

        [HttpPost]
        public ActionResult AddMember(Guid? teamId, Guid? userId)
        {
            try
            {
                service.AddMember(teamId, userId);

                var user = userService.GetUser(userId);
                var team = service.GetTeam(teamId);

                ViewBag.successMessage = user.Forename + " " + user.Surname + " was successfully added to team " + team.Name;

                return RedirectToAction("Index", new { successMessage = ViewBag.successMessage });
            }
            catch
            {
                return View();
            }
        }


        //
        // GET: /Team/Delete/5

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

        //
        // POST: /Team/Delete/5

        [HttpPost]
        public ActionResult Delete(Guid? teamId, FormCollection collection)
        {
            try
            {
                service.DeleteTeam(teamId);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
