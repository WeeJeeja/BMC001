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

        IBookingService service = new BookingService();
        IUserService userService = new UserService();
        ISlotService slotService = new SlotService();
        ITeamService teamService = new TeamService();
        ModelConversitions converter = new ModelConversitions();

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
                    BookingId    = b.BookingId,
                    Date         = b.Date,
                    ResourceName = b.Resource.Name,
                    User         = converter.ConvertUserFromWrapper(b.User),
                    Time         = b.Slot.Time,
                };
                bookings.Add(booking);
            }

            var slots = slotService.GetSlots();

            var timetable = CreateEmptyTimetable();

            #region Add bookings to timetable

            foreach (Booking booking in bookings)
            {
                var entry = timetable.Where(e => e.Time.Equals(booking.Time)).FirstOrDefault();

                switch (booking.Date.DayOfWeek.ToString())
                {
                    case "Monday":
                        {
                            entry.MondayResource = booking.ResourceName;
                            break;
                        }
                    case "Tuesday":
                        {
                            entry.TuesdayResource = booking.ResourceName;
                            break;
                        }
                    case "Wednesday":
                        {
                            entry.WednesdayResource = booking.ResourceName;
                            break;
                        }

                    case "Thursday":
                        {
                            entry.ThursdayResource = booking.ResourceName;
                            break;
                        }

                    case "Friday":
                        {
                            entry.FridayResource = booking.ResourceName;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            #endregion

            return View(timetable);
        }

        //
        // GET: /Booking/Create

        public ActionResult Create()
        {
            #region populate slots
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
            #endregion

            #region populate attendees
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
            #endregion

            #region populate teams
                List<Team> teams = new List<Team>();
                var teamData = teamService.GetTeams();
                
                foreach (wrapper.Team data in teamData)
                {
                    teams.Add(new Team
                    {
                        Name     = data.Name,
                        TeamId   = data.TeamId,
                    });
                    var teamMembers = converter.ConvertUserListFromWrapper(teamService.GetTeamMembers(data.TeamId));
                    foreach (User user in teamMembers)
                    {
                        var team = teams.Where(t => t.TeamId == data.TeamId).FirstOrDefault();
                        team.Members.Add(user);
                    }
                    
                }

            #endregion

                var model = new CreateBooking
                {
                    Slots        = slots,
                    GroupBooking = new GroupBooking
                    {
                        Attendees = attendees,
                        Teams     = teams,
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

        #region HelperMethods

        private List<SelectListItem> getSlots()
        {
            List<SelectListItem> slots = new List<SelectListItem>();
            var slotData = slotService.GetSlots();
            foreach (wrapper.Slot data in slotData)
            {
                slots.Add(new SelectListItem
                {
                    Text = data.Time,
                    Value = data.SlotId.ToString(),
                });
            }

            return slots;
        }

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
                                Time = "09:00 - 10:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
                            });
                            break;
                        }
                    case "10:00 - 11:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time = "10:00 - 11:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
                            });
                            break;
                        }
                    case "11:00 - 12:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time = "11:00 - 12:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
                            });
                            break;
                        }
                    case "12:00 - 13:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time = "12:00 - 13:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
                            });
                            break;
                        }
                    case "13:00 - 14:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time = "13:00 - 14:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
                            });
                            break;
                        }
                    case "14:00 - 15:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time = "14:00 - 15:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
                            });
                            break;
                        }
                    case "15:00 - 16:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time = "15:00 - 16:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
                            });
                            break;
                        }
                    case "16:00 - 17:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time = "16:00 - 17:00",
                                MondayResource = "---",
                                TuesdayResource = "---",
                                WednesdayResource = "---",
                                ThursdayResource = "---",
                                FridayResource = "---",
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

        #endregion

    }
}
