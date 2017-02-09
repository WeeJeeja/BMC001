using wrapper = DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.Models;

namespace HelperMethods
{
    public class ModelConversitions
    {
        public Slot ConvertDataSlotToWrapper(wrapper.Slot entry)
        {
            var slot = new Slot
            {
                SlotId = entry.SlotId,
                Time = entry.Time,
            };

            return slot;
        }

        public Resource ConvertDataResourceToWrapper(wrapper.Resource entry)
        {
            var resource = new Resource
            {
                ResourceId = entry.ResourceId,
                Name = entry.Name,
                Description = entry.Description,
                Category = entry.Category,
                Capacity = entry.Capacity,
                Location = entry.Location,
            };

            return resource;
        }

        public User ConvertDataUserToWrapper(wrapper.User entry)
        {
            var user = new User
            {
                UserId = entry.UserId,
                EmployeeNumber = entry.EmployeeNumber,
                Forename = entry.Forename,
                Surname = entry.Surname,
                JobTitle = entry.JobTitle,
                IsLineManager = entry.IsLineManager,
                IsAdministrator = entry.IsAdministrator,
            };

            return user;
        }

        public ICollection<User> ConvertDataUserListToWrapper(ICollection<wrapper.User> entry)
        {
            var users = new List<User>();

            foreach (wrapper.User data in entry)
            {
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
                users.Add(user);
            };
            return users;
        }
    }
        
}
