using DomainLayer;
using wrapper = DomainLayer.WrapperModels;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationLayer.HelperMethods;
using System.Data;

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

        #endregion

        //
        // GET: /Booking/

        public ActionResult Index()
        {
            ViewBag.Message = "Need to be able to add, edit and delete bookings";

            var userId = Session["UserId"].ToString();

            var data = service.GetThisWeeksBookings(new Guid(userId));
            var bookings = new List<Booking>();

            foreach (wrapper.Booking b in data)
            {
                var booking = new Booking
                {
                    BookingId        = b.BookingId,
                    Date             = b.Date,
                    Resource         = converter.ConvertResourceFromWrapper(b.Resource),
                    User             = converter.ConvertUserFromWrapper(b.User),
                    Slot             = converter.ConvertSlotFromWrapper(b.Slot),
                    BookedBy         = converter.ConvertUserFromWrapper(b.BookedBy),
                    TimetableDisplay = b.Resource.Name,
                };
                bookings.Add(booking);
            }

            var slots = slotService.GetSlots();

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

            var unconfirmedEntries = service.GetThisWeeksUnconfirmedBookings(new Guid(userId));

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

        public ActionResult BookingDetails(Guid? bookingId)
        {
            var booking = service.GetBooking(bookingId);

            var model = new Booking
            {
                BookingId        = booking.BookingId,
                Date             = booking.Date,
                Resource         = converter.ConvertResourceFromWrapper(booking.Resource),
                User             = converter.ConvertUserFromWrapper(booking.User),
                Slot             = converter.ConvertSlotFromWrapper(booking.Slot),
                BookedBy         = converter.ConvertUserFromWrapper(booking.BookedBy),
            };

            return View(model);
        }

        //
        // GET: /Booking/Create

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

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }

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

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }

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

        [HttpPost]
        public ActionResult BookGroup(CreateBooking booking)
        {
            if (!CapacityCheck(booking))
            {
                ModelState.AddModelError("",
                    String.Format("Unable to create the group booking. " + Resource(booking).Name + " has a capacity of " +
                    Resource(booking).Capacity) + ", this booking requires a capacity of " + BookingCapacity(booking));

                booking.Slots = GetSlots();
                booking.GroupBooking.Attendees = GetAttendees();
                booking.GroupBooking.Teams = GetTeams();
                
                return View("Create", booking);
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

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", e);
                return View(booking);
            }

        }

        //
        // GET: /Booking/TeamDetails/5

        public ActionResult TeamDetails(Guid? teamId)
        {
            var data = teamService.GetTeam(teamId);

            var team = converter.ConvertTeamFromWrapper(data);

            return View(team);
        }

        public ActionResult ConfirmGroupBooking(Guid? unconfirmedBookingId)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                service.ConfirmBooking(unconfirmedBookingId);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }


        #region HelperMethods

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
