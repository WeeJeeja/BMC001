using DomainLayer;
using PresentationLayer.HelperMethods;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        #region Fields

        IUserService service           = new UserService();
        IAccountService accountService = new AccountService();
        ModelConversitions converter   = new ModelConversitions();

        #endregion

        /// <summary>
        /// Gets the list of active employees
        /// </summary>
        /// <returns>Employee/Index</returns>
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

        /// <summary>
        /// Gets the employees details
        /// </summary>
        /// <param name="userId">The user Id</param>
        /// <returns>Employee/Details/UserId</returns>
        public ActionResult Details(Guid userId)
        {
            var data = service.GetUser(userId);

            var user = converter.ConvertUserFromWrapper(data);

            return View(user);
        }

        /// <summary>
        /// Gets the create employee page
        /// </summary>
        /// <returns>Employee/Create</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Posts the employee data to create
        /// </summary>
        /// <param name="collection">The empoyee data</param>
        /// <returns>Employee/Index</returns>
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var user = new DomainLayer.WrapperModels.User();
                UpdateModel(user, collection);

                //accountService.AddUserAndAccount(user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets the employee edit page
        /// </summary>
        /// <param name="userId">The user Id</param>
        /// <returns>Employee/Edit/UserId</returns>
        public ActionResult Edit(Guid userId)
        {
            var data = service.GetUser(userId);

            var user = converter.ConvertUserFromWrapper(data);

            return View(user);
        }

        /// <summary>
        /// Posts the updated employee data
        /// </summary>
        /// <param name="userId">The user Id</param>
        /// <param name="collection">Updated employee data</param>
        /// <returns>Employee/Index</returns>
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

        /// <summary>
        /// Gets the employee delete page
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <returns>Employee/Delete</returns>
        public ActionResult Delete(Guid userId)
        {
            var data = service.GetUser(userId);

            var user = converter.ConvertUserFromWrapper(data);

            return View(user);
        }

        /// <summary>
        /// Posts the employee to be deleted
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="collection">The form data</param>
        /// <returns>Employee/Index</returns>
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
