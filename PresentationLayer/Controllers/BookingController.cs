using DomainLayer;
using wrapper = DomainLayer.WrapperModels;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PresentationLayer.HelperMethods;

namespace PresentationLayer.Controllers
{
    public class BookingController : Controller
    {
        #region Fields

        IBookingService service          = new BookingService();
        IUserService userService         = new UserService();
        ISlotService slotService         = new SlotService();
        ITeamService teamService         = new TeamService();
        IResourceService resourceService = new ResourceService();
        ModelConversitions converter     = new ModelConversitions();
        DateCalculations dateCalculator  = new DateCalculations();

        #endregion

        /// <summary>
        /// Gets the allocation timetable and uncofirmed bookings for the logged in users
        /// </summary>
        /// <param name="message">Warning message</param>
        /// <returns>Booking/Index</returns>
        public ActionResult Index(string warningMessage, string successMessage)
        {
            //Used to display warning message on the page
            ViewBag.WarningMessage = warningMessage;

            //Used to display success message on the page
            ViewBag.SuccessMessage = successMessage;

            //Gets the userId of the logged in user
            var userId = Session["UserId"].ToString();

            //Gets all of the bookings belonging to the logged in user for the week
            var data = service.GetThisWeeksBookings(new Guid(userId));

            //Cobverting all of the bookings into view models
            var bookings = new List<Booking>();
            foreach (wrapper.Booking entry in data)
            {
                var booking = converter.ConvertBookingFromWrapper(entry);

                bookings.Add(booking);
            }

            //Gets all of the slots
            var slots = slotService.GetSlots();

            //Creates the empty timetable for the week
            var timetable = new Timetable();
            timetable.TimetableEntries = CreateEmptyTimetable();

            #region Add bookings to timetable

            foreach (Booking booking in bookings)
            {
                var entry = timetable.TimetableEntries.Where(e => e.Time.Equals(booking.Slot.Time)).FirstOrDefault();

                switch (booking.Date.DayOfWeek.ToString())
                {
                    case "Monday":
                        {
                            entry.MondayResource = booking;
                            break;
                        }
                    case "Tuesday":
                        {
                            entry.TuesdayResource = booking;
                            break;
                        }
                    case "Wednesday":
                        {
                            entry.WednesdayResource = booking;
                            break;
                        }

                    case "Thursday":
                        {
                            entry.ThursdayResource = booking;
                            break;
                        }

                    case "Friday":
                        {
                            entry.FridayResource = booking;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            #endregion

            //Gets of the logged in users unconfirmed bookings for the week
            var unconfirmedEntries = service.GetThisWeeksUnconfirmedBookings(new Guid(userId));

            //Converts the uncofirmed bookings to view models
            foreach (wrapper.Booking unconfirmedBooking in unconfirmedEntries)
            {
                timetable.UnconfirmedEntries.Add(new UnconfirmedEntry
                    {
                        UnconfirmedBookingId   = unconfirmedBooking.BookingId,
                        Date        = unconfirmedBooking.Date,
                        Slot        = converter.ConvertSlotFromWrapper(unconfirmedBooking.Slot),
                        StartTime   = unconfirmedBooking.Slot.Time,
                        Resource    = converter.ConvertResourceFromWrapper(unconfirmedBooking.Resource),
                        Creator     = converter.ConvertUserFromWrapper(unconfirmedBooking.BookedBy),
                        BookedBy    = unconfirmedBooking.BookedBy.Forename + " " + unconfirmedBooking.BookedBy.Surname + " - " + unconfirmedBooking.BookedBy.JobTitle,
                    });
            }

            return View(timetable);
        }

        /// <summary>
        /// Gets the bookings details
        /// </summary>
        /// <param name="bookingId">The booking Id</param>
        /// <returns>Booking/BookingDetails/BookingId</returns>
        public ActionResult BookingDetails(Guid? bookingId)
        {
            var booking = service.GetBooking(bookingId);

            var model = converter.ConvertBookingFromWrapper(booking);

            return View(model);
        }

        /// <summary>
        /// Getsthe create booking page
        /// </summary>
        /// <returns>Booking/Create</returns>
        public ActionResult Create()
        {
            var model = new CreateBooking
            {
                Slots        = GetSlots(),
                GroupBooking = new GroupBooking
                {
                    Attendees = GetAttendees(),
                    Teams     = GetTeams(),
                }
            };
            
            return View(model);
        }

        #region Single Booking

        /// <summary>
        /// Retrieves available resources for a single booking
        /// </summary>
        /// <param name="booking">The booking Id</param>
        /// <returns>Booking/Create/SingleBooking</returns>
        public PartialViewResult RetrieveAvailableResources(CreateBooking booking)
        {
            var availableResources = service.GetAvailableResources(booking.SingleBooking.Date, booking.SingleBooking.Slot);

            var rs = new ResourceService();
            var resources = converter.ConvertResourceListFromWrapper(availableResources);
            booking.Resources = resources;

            var time = slotService.GetSlot(booking.SingleBooking.Slot).StartTime;

            ViewBag.Message = booking.Resources.Count() + " resources are available on " +
                booking.SingleBooking.Date.ToShortDateString() + 
                " at " + string.Format("{0:hh\\:mm}", time);

            return PartialView("_resources", booking);
        }

        /// <summary>
        /// Creates a single booking
        /// </summary>
        /// <param name="booking">The booking to be added</param>
        /// <returns>Booking/Index</returns>
        [HttpPost]
        public ActionResult Book(CreateBooking booking)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                booking.User = converter.ConvertUserFromWrapper(user);

                var convertedBooking = converter.ConvertSingleBookingToWrapper(booking);

                service.AddBooking(convertedBooking);

                return RedirectToAction("Index", new { successMessage = "Booking successfully added!"});
            }
            catch
            {
                return View("Create");
            }
        }

        #endregion

        #region BlockBooking

        /// <summary>
        /// Gets the available resources for a block booking
        /// </summary>
        /// <param name="booking">The booking</param>
        /// <returns>Booking/Create/BlockBooking</returns>
        public PartialViewResult RetrieveAvailableResourcesForBlockBooking(CreateBooking booking)
        {
            var availableResources = service.GetAvailableResourcesForBlockBooking(
                booking.BlockBooking.StartDate,
                booking.BlockBooking.EndDate,
                booking.BlockBooking.StartSlot,
                booking.BlockBooking.EndSlot);

            var rs = new ResourceService();
            var resources = converter.ConvertResourceListFromWrapper(availableResources);
            booking.Resources = resources;

            var startTime = slotService.GetSlot(booking.BlockBooking.StartSlot).StartTime;
            var endTime = slotService.GetSlot(booking.BlockBooking.EndSlot).EndTime;

            ViewBag.Message = booking.Resources.Count() + " resources are available from " +
                booking.BlockBooking.StartDate.ToShortDateString() + " to " + booking.BlockBooking.EndDate.ToShortDateString() +
                " between " + string.Format("{0:hh\\:mm}", startTime) + " - " + string.Format("{0:hh\\:mm}", endTime);

            return PartialView("_blockResources", booking);
        }

        /// <summary>
        /// Creates a booking for each time slot in the date range
        /// </summary>
        /// <param name="booking">The booking</param>
        /// <returns>Booking/Index</returns>
        [HttpPost]
        public ActionResult BookBlock(CreateBooking booking)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                service.AddBlockBooking(
                    booking.BlockBooking.StartDate,
                    booking.BlockBooking.EndDate,
                    booking.BlockBooking.StartSlot,
                    booking.BlockBooking.EndSlot,
                    booking.Resource,
                    user.UserId);

                return RedirectToAction("Index", new { successMessage = "Block bookings successfully added!"});
            }
            catch
            {
                return View("Create");
            }
        }

        #endregion

        #region GroupBooking

        /// <summary>
        /// Gets the available resources for a group booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        public PartialViewResult RetrieveAvailableResourcesForGroupBooking(CreateBooking booking)
        {
            var availableResources = service.GetAvailableResourcesForGroupBooking(
                booking.GroupBooking.Date,
                booking.GroupBooking.StartTime,
                booking.GroupBooking.EndTime,
                booking.GroupBooking.Capacity);

            var rs = new ResourceService();
            var resources = converter.ConvertResourceListFromWrapper(availableResources);
            booking.Resources = resources;

            var startTime = slotService.GetSlot(booking.GroupBooking.StartTime).StartTime;
            var endTime = slotService.GetSlot(booking.GroupBooking.EndTime).EndTime;

            ViewBag.Message = booking.Resources.Count() + " resources are available on " +
                booking.GroupBooking.Date.ToShortDateString() +
                " between " + string.Format("{0:hh\\:mm}", startTime) + " - " + string.Format("{0:hh\\:mm}", endTime);

            return PartialView("_groupResources", booking);
        }

        /// <summary>
        /// Create an unconfirmed booking for eevery attendee in the group booking
        /// </summary>
        /// <param name="booking">The booking</param>
        /// <returns>Booking/Index</returns>
        [HttpPost]
        public ActionResult BookGroup(CreateBooking booking)
        {
            if (!CapacityCheck(booking))
            {
                var warningMessage = "Unable to create the group booking. " + Resource(booking).Name + " has a capacity of " +
                    Resource(booking).Capacity + ", this booking requires a capacity of " + BookingCapacity(booking);

                return RedirectToAction("Index", new { warningMessage = warningMessage });
            }
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                service.AddGroupBooking(
                    booking.GroupBooking.Date,
                    booking.GroupBooking.SelectedAttendees.ToList(),
                    booking.GroupBooking.SelectedTeams.ToList(),
                    booking.GroupBooking.StartTime,
                    booking.GroupBooking.EndTime,
                    booking.Resource,
                    user.UserId);

                return RedirectToAction("Index", new { successMessage = "Group booking successfully added!" });
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e);
                return View(booking);
            }

        }

        /// <summary>
        /// Gets the team details
        /// </summary>
        /// <param name="teamId">The team Id</param>
        /// <returns>Team/Details</returns>
        public ActionResult TeamDetails(Guid? teamId)
        {
            var data = teamService.GetTeam(teamId);

            var team = converter.ConvertTeamFromWrapper(data);

            return View(team);
        }

        /// <summary>
        /// Confirms a group booking
        /// </summary>
        /// <param name="unconfirmedBookingId">The booking</param>
        /// <returns>Booking/Index</returns>
        public ActionResult ConfirmGroupBooking(Guid? unconfirmedBookingId)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                service.ConfirmBooking(unconfirmedBookingId);

                return RedirectToAction("Index", new { successMessage = "Group booking successfully confirmed!" });
            }
            catch
            {
                return View("Create");
            }
        }

        #endregion

        /// <summary>
        /// Deletes a booking from a users allocation timetable
        /// </summary>
        /// <param name="bookingId">The booking</param>
        /// <returns>Booking/Index</returns>
        public ActionResult Delete(Guid? bookingId)
        {
            try
            {
                service.DeleteBooking(bookingId);

                return RedirectToAction("Index", new { successMessage = "Booking successfully deleted!" });
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets the add attendee to group booking page
        /// </summary>
        /// <param name="bookingId">The booking Id</param>
        /// <returns>Booking/AddAttendee/BookingId</returns>
        public ActionResult AddAttendee(Guid? bookingId)
        {
            var booking = converter.ConvertBookingFromWrapper(service.GetBooking(bookingId));

            var users = converter.ConvertUserListFromWrapper(userService.GetUsers());

            var attendees = booking.ConfirmedAttendees.Concat(booking.UnconfirmedAttendees).ToList();

            var potentialAttendees = new List<User>();

                foreach (User user in users)
                {
                    var test = attendees.Where(u => u.UserId == user.UserId).FirstOrDefault();
                    if (test == null)
                    {
                        potentialAttendees.Add(user);
                    }
                }
            
            var model = new UpdateBooking
            {
                Booking            = booking,
                PotentialAttendees = potentialAttendees,
            };

            return View(model);
        }

        /// <summary>
        /// Adds an attendee to an exisiting group booking
        /// </summary>
        /// <param name="bookingId">The bookingId</param>
        /// <param name="userId">The userId</param>
        /// <returns>Booking/Index</returns>
        [HttpPost]
        public ActionResult AddAttendee(Guid? bookingId, Guid? userId)
        {
            try
            {
                var booking = service.GetBooking(bookingId);

                if ((booking.ConfirmedAttendees.Count() + booking.UnconfirmedAttendees.Count()) >= booking.Resource.Capacity)
                {
                    var warningMessage = "Unable to add attendee to booking. " + booking.Resource.Name + " has a capacity of " +
                        booking.Resource.Capacity + ". Confirmed attendees: " + booking.ConfirmedAttendees.Count() + " Unconfirmed attendees: " + booking.UnconfirmedAttendees.Count();

                    return RedirectToAction("Index", new { warningMessage = warningMessage });
                }

                service.AddAttendeeToGroupBooking(bookingId, userId);

                return RedirectToAction("Index", new { successMessage = "Attendee successfully added to group booking!" });
            }
            catch
            {
                return RedirectToAction("Index", new { warningMessage = "Opps...Something went wrong, please try again or contact the tech team." });
            }
        }

        /// <summary>
        /// Auto books a resource for every empty timeslot in the users timetable
        /// </summary>
        /// <returns>Booking/Index</returns>
        public ActionResult AutoBook()
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                var date = dateCalculator.FindStartDate(DateTime.Today);

                var result = service.AutoBook(date, user.UserId);

                if (!result)
                    return RedirectToAction("Index", new { warningMessage = "No single resource is available to block book for the week." });

                return RedirectToAction("Index", new { successMessage = "Auto bookings successfully added!" });
            }
            catch
            {
                return RedirectToAction("Index", new { warningMessage = "Something went wrong, please try again or contact the tech team." });
            }
        }

        #region HelperMethods

        /// <summary>
        /// Creates an empty allocation timetable
        /// </summary>
        /// <returns>The timetable</returns>
        private List<TimetableEntry> CreateEmptyTimetable()
        {
            var slots = slotService.GetSlots();
            var timetable = new List<TimetableEntry>();
            
            foreach (wrapper.Slot slot in slots)
            {
                switch (slot.Time)
                {
                    case "09:00 - 10:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "09:00 - 10:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    case "10:00 - 11:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "10:00 - 11:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    case "11:00 - 12:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "11:00 - 12:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    case "12:00 - 13:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "12:00 - 13:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    case "13:00 - 14:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "13:00 - 14:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    case "14:00 - 15:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "14:00 - 15:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    case "15:00 - 16:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "15:00 - 16:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    case "16:00 - 17:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "16:00 - 17:00",
                                MondayResource    = new Booking(),
                                TuesdayResource   = new Booking(),
                                WednesdayResource = new Booking(),
                                ThursdayResource  = new Booking(),
                                FridayResource    = new Booking(),
                            });
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return timetable;
        }

        /// <summary>
        /// Checks if a resource is big enough for a group booking
        /// </summary>
        /// <param name="booking">The booking</param>
        /// <returns>True or false</returns>
        private bool CapacityCheck(CreateBooking booking)
        {
            var resource = resourceService.GetResource(booking.Resource);
            var attendeeCount = booking.GroupBooking.SelectedAttendees.Count();
            var teamMemberCount = 0;
            foreach (string teamId in booking.GroupBooking.SelectedTeams)
            {
                var team = teamService.GetTeam(new Guid(teamId));
                teamMemberCount += team.Members.Count();
            }

            if (resource.Capacity >= (attendeeCount + teamMemberCount)) return true;
            return false;
        }

        /// <summary>
        /// Gets the capcity a booking requires 
        /// Teams memebers + attendees
        /// </summary>
        /// <param name="booking">The booking</param>
        /// <returns>The number of attendess in the booking</returns>
        private int BookingCapacity(CreateBooking booking)
        {
            var attendeeCount = booking.GroupBooking.SelectedAttendees.Count();
            var teamMemberCount = 0;
            foreach (string teamId in booking.GroupBooking.SelectedTeams)
            {
                var team = teamService.GetTeam(new Guid(teamId));
                teamMemberCount += team.Members.Count();
            }

            return attendeeCount + teamMemberCount; 
        }

        private Resource Resource(CreateBooking booking)
        {
            var resource = converter.ConvertResourceFromWrapper(resourceService.GetResource(booking.Resource));

            return resource;
        }

        private List<Slot> GetSlots()
        {
            List<Slot> slots = new List<Slot>();
                var slotData = slotService.GetSlots();
                foreach (wrapper.Slot data in slotData)
                {
                    slots.Add(new Slot
                    {
                        Time = data.Time,
                        SlotId = data.SlotId,
                    });
                }
            return slots;
        }

        private List<User> GetAttendees()
        {
            List<User> attendees = new List<User>();
                var attendeeData = userService.GetUsers();
                foreach (wrapper.User data in attendeeData)
                {
                    attendees.Add(new User
                    {
                        EmployeeNumber = data.EmployeeNumber,
                        Forename       = data.Forename,
                        Surname        = data.Surname,
                        JobTitle       = data.JobTitle,
                        UserId         = data.UserId,
                    });
                }
            return attendees;
        }

        /// <summary>
        /// Gets all of the teams and their memebers
        /// </summary>
        /// <returns>A list of teams</returns>
        private List<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();
            var teamData = teamService.GetTeams();

            foreach (wrapper.Team data in teamData)
            {
                teams.Add(new Team
                {
                    Name = data.Name,
                    TeamId = data.TeamId,
                });
                var teamMembers = converter.ConvertUserListFromWrapper(teamService.GetTeamMembers(data.TeamId));
                foreach (User user in teamMembers)
                {
                    var team = teams.Where(t => t.TeamId == data.TeamId).FirstOrDefault();
                    team.Members.Add(user);
                }
            }
            return teams;
        }
        
        #endregion

    }
}
