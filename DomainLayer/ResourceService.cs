using DataLayer;
using DomainLayer.WrapperModels;
using HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer
{
    public class ResourceService : DomainLayer.IResourceService
    {
        #region Fields

        ModelConversitions converter = new ModelConversitions();

        #endregion

        /// <summary>
        /// Gets a list of active resources
        /// </summary>
        /// <param name="date">the date to get the active resources until</param>
        /// <returns>the active resources</returns>
        public List<Resource> GetResources(DateTime? date = null)
        {
            var db = new ReScrumEntities();

            if (date == null) date = DateTime.Today;

            //get active resources
            var resourceData = db.Resources.Where(r => r.CancellationDate == null || r.CancellationDate < date).ToList();
            var resources = new List<Resource>();

            foreach (DataLayer.Models.Resource data in resourceData)
            {
                var resource = converter.ConvertDataResourceToWrapper(data);
                resources.Add(resource);
            }
            return resources;
        }

        /// <summary>
        /// Gets the resource using the resourceId
        /// </summary>
        /// <returns>Returns the resource</returns>
        public Resource GetResource(Guid? resourceId)
        {
            var db = new ReScrumEntities();

            var data = db.Resources.Where(u => u.ResourceId == resourceId).FirstOrDefault();

            var resource = converter.ConvertDataResourceToWrapper(data);

            return resource;
        }

        /// <summary>
        /// Adds a new resource to the database
        /// </summary>
        /// <param name="resource">The new resource to be added</param>
        public void AddResource(Resource resource)
        {
            var db = new ReScrumEntities();

            var newResource = new DataLayer.Models.Resource
            {
                ResourceId  = resource.ResourceId,
                Name        = resource.Name,
                Description = resource.Description,
                Category    = resource.Category,
                Capacity    = resource.Capacity,
                Location    = resource.Location,
            };

            db.Resources.Add(newResource);

            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing resource
        /// </summary>
        /// <param name="data">The new resource details</param>
        public void UpdateResource(Resource data)
        {
            var db = new ReScrumEntities();

            var resource = db.Resources.Where(u => u.ResourceId == data.ResourceId).FirstOrDefault();

            resource.ResourceId       = data.ResourceId;
            resource.Name             = data.Name;
            resource.Description      = data.Description;
            resource.Category         = data.Category;
            resource.Capacity         = data.Capacity;
            resource.Location         = data.Location;
            resource.CancellationDate = data.CancellationDate;

            db.SaveChanges();
        }

        /// <summary>
        /// Deletes a resource from the database
        /// </summary>
        /// <param name="data">The resource to be deleted</param>
        public void DeleteResource(Guid? resourceId)
        {
            var db = new ReScrumEntities();

            var resource = db.Resources.Where(u => u.ResourceId == resourceId).FirstOrDefault();

            resource.CancellationDate = DateTime.Today;

            db.SaveChanges();
        }
    }
}
