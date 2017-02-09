using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface IResourceService
    {
        /// <summary>
        /// Gets a list of all resources
        /// </summary>
        /// <returns>A list of all resources</returns>
        List<Resource> GetResources();

        /// <summary>
        /// Gets the resource using the resourceId
        /// </summary>
        /// <param name="resourceId">The resource to be retrived</param>
        /// <returns>Returns the resource</returns>
        Resource GetResource(Guid resourceId);

        /// <summary>
        /// Adds a new resource to the database
        /// </summary>
        /// <param name="resource">The new resource to be added</param>
        void AddResource(Resource resource);

        /// <summary>
        /// Updates an existing resource
        /// </summary>
        /// <param name="data">The new resource details</param>
        void UpdateResource(Resource data);

        /// <summary>
        /// Deletes a resource from the database
        /// </summary>
        /// <param name="data">The resource to be deleted</param>
        void DeleteResource(Guid? resourceId);
    }
}
