using DomainLayer;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        IUserService service = new UserService();


        //
        // GET: /EmployeeController/

        public ActionResult Index()
        {
            ViewBag.Message = "Need to be able to add an employee and search for existing ones";

            var data = service.GetUsers();
            var users = new List<User>();

            foreach (DomainLayer.WrapperModels.User u in data)
            {
                var user = new User
                {
                    UserId          = u.UserId,
                    Forename        = u.Forename,
                    Surname         = u.Surname,
                    JobTitle        = u.JobTitle,
                    IsLineManager   = u.IsLineManager,
                    IsAdministrator = u.IsAdministrator,

                };
                users.Add(user);
            }

            return View(users);
        }

        //
        // GET: /EmployeeController/Details/5

        public ActionResult Details(Guid userId)
        {
            var data = service.GetUser(userId);

            var user = new User
            {
                UserId          = data.UserId,
                EmployeeNumber  = data.EmployeeNumber,
                Forename        = data.Forename,
                Surname         = data.Surname,
                JobTitle        = data.JobTitle,
                IsLineManager   = data.IsLineManager,
                IsAdministrator = data.IsAdministrator,
            };

            return View(user);
        }

        //
        // GET: /EmployeeController/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /EmployeeController/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var user = new DomainLayer.WrapperModels.User();
                UpdateModel(user, collection);

                service.AddUser(user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /EmployeeController/Edit/id

        public ActionResult Edit(Guid userId)
        {
            var data = service.GetUser(userId);

            var user = new User
            {
                EmployeeNumber  = data.EmployeeNumber,
                Forename        = data.Forename,
                Surname         = data.Surname,
                JobTitle        = data.JobTitle,
                IsLineManager   = data.IsLineManager,
                IsAdministrator = data.IsAdministrator,
            };

            return View(user);
        }

        //
        // POST: /EmployeeController/Edit/userId

        [HttpPost]
        public ActionResult Edit(Guid userId, FormCollection collection)
        {
            try
            {
                var user = new DomainLayer.WrapperModels.User();
                UpdateModel(user, collection);

                service.UpdateUser(user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /EmployeeControllerapi/Delete/5

        public ActionResult Delete(Guid userId)
        {
            var data = service.GetUser(userId);

            var user = new User
            {
                EmployeeNumber  = data.EmployeeNumber,
                Forename        = data.Forename,
                Surname         = data.Surname,
                JobTitle        = data.JobTitle,
                IsLineManager   = data.IsLineManager,
                IsAdministrator = data.IsAdministrator,
            };

            return View(user);
        }

        //
        // POST: /EmployeeControllerapi/Delete/5

        [HttpPost]
        public ActionResult Delete(Guid userId, FormCollection collection)
        {
            try
            {
                service.DeleteUser(userId);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
