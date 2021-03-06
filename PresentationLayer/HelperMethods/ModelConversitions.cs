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

        #region Convert View models from wrappers

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
                Email           = entry.Email,
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
                    Email           = data.Email,
                    JobTitle        = data.JobTitle,
                    IsLineManager   = data.IsLineManager,
                    IsAdministrator = data.IsAdministrator,
                };
                users.Add(user);
            };
            return users;
        }

        public Team ConvertTeamFromWrapper(wrapper.Team entry)
        {
            var team = new Team
            {
                TeamId = entry.TeamId,
                Name   = entry.Name,
                Colour = entry.Colour,
            };

            foreach (wrapper.User member in entry.Members)
            {
                team.Members.Add(ConvertUserFromWrapper(member));
            }

            return team;
        }

        public ICollection<Team> ConvertTeamsFromWrapper(ICollection<wrapper.Team> entries)
        {
            var teams = new List<Team>();

            foreach(wrapper.Team entry in entries)
            {
                var team = new Team
                {
                    TeamId = entry.TeamId,
                    Name   = entry.Name,
                    Colour = entry.Colour,
                };

                foreach (wrapper.User member in entry.Members)
                {
                    team.Members.Add(ConvertUserFromWrapper(member));
                }

                teams.Add(team);
            }

            return teams;
        }

        public Booking ConvertBookingFromWrapper(wrapper.Booking entry)
        {
            var model = new Booking
            {
                BookingId            = entry.BookingId,
                Date                 = entry.Date,
                Resource             = ConvertResourceFromWrapper(entry.Resource),
                User                 = ConvertUserFromWrapper(entry.User),
                Slot                 = ConvertSlotFromWrapper(entry.Slot),
                BookedBy             = ConvertUserFromWrapper(entry.BookedBy),
                GroupBooking         = entry.GroupBooking,
                TimetableDisplay     = entry.Resource.Name,
                ConfirmedAttendees   = ConvertUserListFromWrapper(entry.ConfirmedAttendees),
                UnconfirmedAttendees = ConvertUserListFromWrapper(entry.UnconfirmedAttendees),
            };

            return model;
        }

        #endregion

        #region Convert View models to wrappers

        public wrapper.User ConvertUserToWrapper(User entry)
        {
            var user = new wrapper.User
            {
                UserId           = entry.UserId,
                EmployeeNumber   = entry.EmployeeNumber,
                Forename         = entry.Forename,
                Surname          = entry.Surname,
                Email            = entry.Email,
                JobTitle         = entry.JobTitle,
                IsLineManager    = entry.IsLineManager,
                IsAdministrator  = entry.IsAdministrator,
                CancellationDate = entry.CancellationDate,
            };

            return user;
        }

        public wrapper.User ConvertRegistrationToWrapper(RegisterModel entry)
        {
            var user = new wrapper.User
            {
                EmployeeNumber   = entry.EmployeeNumber,
                Forename         = entry.Forename,
                Surname          = entry.Surname,
                Email            = entry.Email,
                JobTitle         = entry.JobTitle,
                Team             = entry.Team,
            };

            return user;
        }

        public wrapper.Booking ConvertBookingToWrapper(Booking entry)
        {
            var slot = slotService.GetSlot(entry.Slot.SlotId);
            var resource = resourceService.GetResource(entry.Resource.ResourceId);

            var booking = new wrapper.Booking
            {
                BookingId    = entry.BookingId,
                Date         = entry.Date,
                Slot         = slot,
                Resource     = resource,
                User         = ConvertUserToWrapper(entry.User),
                BookedBy     = ConvertUserToWrapper(entry.BookedBy),
                GroupBooking = entry.GroupBooking,
            };

            return booking;
        }

        public wrapper.Booking ConvertSingleBookingToWrapper(CreateBooking entry)
        {
            var slot     = slotService.GetSlot(entry.SingleBooking.Slot);
            var resource = resourceService.GetResource(entry.Resource);

            var booking = new wrapper.Booking
            {
                Date          = entry.SingleBooking.Date,
                Slot          = slot,
                Resource      = resource,
                User          = ConvertUserToWrapper(entry.User),
            };

            return booking;
        }

        public wrapper.Slot ConvertSlotToWrapper(Slot entry)
        {
            var slot = new wrapper.Slot
            {
                SlotId  = entry.SlotId,
                Time    = entry.Time,
            };

            return slot;
        }

        public wrapper.Resource ConvertResourceToWrapper(Resource entry)
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

        #endregion
    }
        
}
