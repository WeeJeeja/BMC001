﻿using DomainLayer;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class ResourceController : Controller
    {

        #region Fields

        IResourceService service = new ResourceService();

        #endregion

        //
        // GET: /Resource/

        public ActionResult Index()
        {
            ViewBag.Message = "Add, edit or delete a resource";

            var data = service.GetResources();
            var resources = new List<Resource>();

            foreach (DomainLayer.WrapperModels.Resource r in data)
            {
                var resource = new Resource
                {
                    ResourceId  = r.ResourceId,
                    Name        = r.Name,
                    Description = r.Description,
                    Capacity    = r.Capacity,
                    Category    = r.Category,
                    Location    = r.Location,
                };
                resources.Add(resource);
            }
            return View(resources);
        }

        //
        // GET: /Resource/Details/5

        public ActionResult Details(Guid resourceId)
        {
            var data = service.GetResource(resourceId);

            var resource = new Resource
            {
                ResourceId = data.ResourceId,
                Name = data.Name,
                Description = data.Description,
                Capacity = data.Capacity,
                Category = data.Category,
                Location = data.Location,
            };

            return View(resource);
        }

        //
        // GET: /Resource/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Resource/Create

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

        //
        // GET: /Resource/Edit/5

        public ActionResult Edit(Guid resourceId)
        {
            var data = service.GetResource(resourceId);

            var resource = new Resource
            {
                ResourceId  = data.ResourceId,
                Name        = data.Name,
                Description = data.Description,
                Capacity    = data.Capacity,
                Category    = data.Category,
                Location    = data.Location,
            };

            return View(resource);
        }

        //
        // POST: /Resource/Edit/5

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

        //
        // GET: /Resource/Delete/5

        public ActionResult Delete(Guid resourceId)
        {
            var data = service.GetResource(resourceId);

            var resource = new Resource
            {
                ResourceId  = data.ResourceId,
                Name        = data.Name,
                Description = data.Description,
                Capacity    = data.Capacity,
                Category    = data.Category,
                Location    = data.Location,
            };

            return View(resource);
        }

        //
        // POST: /Resource/Delete/5

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
