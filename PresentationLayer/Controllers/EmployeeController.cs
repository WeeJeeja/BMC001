using DomainLayer;
using PresentationLayer.HelperMethods;
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
        #region Fields

        IUserService service = new UserService();
        ModelConversitions converter = new ModelConversitions();

        #endregion

        //
        // GET: /EmployeeController/

        public ActionResult Index()
        {
            ViewBag.Message = "Need to be able to add an employee and search for existing ones";

            var userList = service.GetUsers();
            var users = new List<User>();

            foreach (DomainLayer.WrapperModels.User data in userList)
            {
                var user = converter.ConvertUserFromWrapper(data);
                users.Add(user);
            }

            return View(users);
        }

        //
        // GET: /EmployeeController/Details/5

        public ActionResult Details(Guid userId)
        {
            var data = service.GetUser(userId);

            var user = converter.ConvertUserFromWrapper(data);

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

            var user = converter.ConvertUserFromWrapper(data);

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

            var user = converter.ConvertUserFromWrapper(data);

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
