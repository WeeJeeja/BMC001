using wrapper = DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data = DataLayer.Models;

namespace HelperMethods
{
    public class ModelConversitions
    {
        public wrapper.Slot ConvertDataSlotToWrapper(data.Slot entry)
        {
            var slot = new wrapper.Slot
            {
                SlotId = entry.SlotId,
                Time   = entry.Time,
            };

            return slot;
        }

        public wrapper.Resource ConvertDataResourceToWrapper(data.Resource entry)
        {
            var resource = new wrapper.Resource
            {
                ResourceId  = entry.ResourceId,
                Name        = entry.Name,
                Description = entry.Description,
                Category    = entry.Category,
                Capacity    = entry.Capacity,
                Location    = entry.Location,
            };

            return resource;
        }

        public ICollection<wrapper.Resource> ConvertDataResourceListToWrapper(ICollection<data.Resource> entry)
        {
            var resources = new List<wrapper.Resource>();

            foreach (data.Resource data in entry)
            {
                var resource = new wrapper.Resource
                {
                    ResourceId  = data.ResourceId,
                    Name        = data.Name,
                    Description = data.Description,
                    Category    = data.Category,
                    Capacity    = data.Capacity,
                    Location    = data.Location,
                };
                resources.Add(resource);
            };
            return resources;
        }

        public wrapper.User ConvertDataUserToWrapper(data.User entry)
        {
            var user = new wrapper.User
            {
                UserId          = entry.UserId,
                EmployeeNumber  = entry.EmployeeNumber,
                Forename        = entry.Forename,
                Surname         = entry.Surname,
                JobTitle        = entry.JobTitle,
                IsLineManager   = entry.IsLineManager,
                IsAdministrator = entry.IsAdministrator,
            };

            return user;
        }


        //**delete this
        public ICollection<wrapper.User> ConvertDataUserListToWrapper(ICollection<data.User> entry)
        {
            var users = new List<wrapper.User>();

            foreach (data.User data in entry)
            {
                var user = new wrapper.User
                {
                    UserId = data.UserId,
                    EmployeeNumber = data.EmployeeNumber,
                    Forename = data.Forename,
                    Surname = data.Surname,
                    JobTitle = data.JobTitle,
                    IsLineManager = data.IsLineManager,
                    IsAdministrator = data.IsAdministrator,
                };
                users.Add(user);
            };
            return users;
        }

        //public data.Slot ConvertSlotFromWrapper(wrapper.Slot entry)
        //{
        //    var slot = new data.Slot
        //    {
        //        SlotId = entry.SlotId,
        //        Time   = entry.Time,
        //    };

        //    return slot;
        //}

        //public data.Resource ConvertResourceFromWrapper(wrapper.Resource entry)
        //{
        //    var resource = new data.Resource
        //    {
        //        ResourceId  = entry.ResourceId,
        //        Name        = entry.Name,
        //        Description = entry.Description,
        //        Category    = entry.Category,
        //        Capacity    = entry.Capacity,
        //        Location    = entry.Location,
        //    };

        //    return resource;
        //}

        //public data.User ConvertUserFromWrapper(wrapper.User entry)
        //{
        //    var user = new data.User
        //    {
        //        UserId          = entry.UserId,
        //        EmployeeNumber  = entry.EmployeeNumber,
        //        Forename        = entry.Forename,
        //        Surname         = entry.Surname,
        //        JobTitle        = entry.JobTitle,
        //        IsLineManager   = entry.IsLineManager,
        //        IsAdministrator = entry.IsAdministrator,
        //    };

        //    return user;
        //}
    }
        
}
