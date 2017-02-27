﻿using wrapper = DomainLayer.WrapperModels;
using System.Collections.Generic;
using PresentationLayer.Models;
using DomainLayer;

namespace PresentationLayer.HelperMethods
{
    public class ModelConversitions
    {
        ISlotService slotService = new SlotService();
        IResourceService resourceService = new ResourceService();

        #region Convert View models to wrappers
        public Slot ConvertSlotFromWrapper(wrapper.Slot entry)
        {
            var slot = new Slot
            {
                SlotId = entry.SlotId,
                Time = entry.Time,
            };

            return slot;
        }

        public Resource ConvertResourceFromWrapper(wrapper.Resource entry)
        {
            var resource = new Resource
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

        public ICollection<Resource> ConvertResourceListFromWrapper(ICollection<wrapper.Resource> entry)
        {
            var resources = new List<Resource>();

            foreach (wrapper.Resource data in entry)
            {
                var resource = new Resource
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

        public User ConvertUserFromWrapper(wrapper.User entry)
        {
            var user = new User
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

        public ICollection<User> ConvertUserListFromWrapper(ICollection<wrapper.User> entry)
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

        #endregion

        #region Convert View models to wrappers

        public wrapper.User ConvertUserToWrapper(User entry)
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

        public wrapper.Booking ConvertBookingToWrapper(Booking entry)
        {
            var slot = slotService.GetSlot(entry.Slot);
            var resource = resourceService.GetResource(entry.Resource);


            var booking = new wrapper.Booking
            {
                BookingId = entry.BookingId,
                Capacity  = entry.Capacity,
                Slot      = slot,
                Resource  = resource,
                User      = ConvertUserToWrapper(entry.User),
            };

            return booking;
        }


        #endregion
    }
        
}
