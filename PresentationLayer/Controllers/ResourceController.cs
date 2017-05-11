using DomainLayer;
using PresentationLayer.HelperMethods;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class ResourceController : Controller
    {

        #region Fields

        IResourceService service     = new ResourceService();
        ModelConversitions converter = new ModelConversitions();

        #endregion

        /// <summary>
        /// Gets the list of active resources
        /// </summary>
        /// <returns>Resources/Index</returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Add, edit or delete a resource";

            var resourceData = service.GetResources();
            var resources = new List<Resource>();

            foreach (DomainLayer.WrapperModels.Resource data in resourceData)
            {
                var resource = converter.ConvertResourceFromWrapper(data);
                resources.Add(resource);
            }
            return View(resources);
        }

        /// <summary>
        /// Gets the resource details page
        /// </summary>
        /// <param name="resourceId">The resource Id</param>
        /// <returns>Resource/Details/ResourceId</returns>
        public ActionResult Details(Guid resourceId)
        {
            var data = service.GetResource(resourceId);

            var resource = converter.ConvertResourceFromWrapper(data);

            return View(resource);
        }

        /// <summary>
        /// Gets the resource create page
        /// </summary>
        /// <returns>Resource/Create</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Posts the create data for the resource
        /// </summary>
        /// <param name="collection">The resource data</param>
        /// <returns>Resource/Index</returns>
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var resource = new DomainLayer.WrapperModels.Resource();
                UpdateModel(resource, collection);

                service.AddResource(resource);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets the dit page for the resource
        /// </summary>
        /// <param name="resourceId">The resource Id</param>
        /// <returns>Resource/Edit/ResourceId</returns>
        public ActionResult Edit(Guid resourceId)
        {
            var data = service.GetResource(resourceId);

            var resource = converter.ConvertResourceFromWrapper(data);

            return View(resource);
        }

        /// <summary>
        /// Posts the updated resource details
        /// </summary>
        /// <param name="resourceId">The reosurce Id</param>
        /// <param name="collection">Updated resource details</param>
        /// <returns>Resource/Index</returns>
        [HttpPost]
        public ActionResult Edit(Guid resourceId, FormCollection collection)
        {
            try
            {
                var resource = new DomainLayer.WrapperModels.Resource();
                UpdateModel(resource, collection);

                service.UpdateResource(resource);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets the delete page for the resource
        /// </summary>
        /// <param name="resourceId">The resource Id</param>
        /// <returns>Resource/Delete/ResourceId</returns>
        public ActionResult Delete(Guid resourceId)
        {
            var data = service.GetResource(resourceId);

            var resource = converter.ConvertResourceFromWrapper(data);

            return View(resource);
        }

        /// <summary>
        /// Posts the resource to be deleted
        /// </summary>
        /// <param name="resourceId">The resource Id</param>
        /// <param name="collection">The resource data</param>
        /// <returns>Resource/Index</returns>
        [HttpPost]
        public ActionResult Delete(Guid resourceId, FormCollection collection)
        {
            try
            {
                service.DeleteResource(resourceId);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
