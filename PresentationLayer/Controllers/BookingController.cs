using DomainLayer;
using wrapper = DomainLayer.WrapperModels;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationLayer.HelperMethods;

namespace PresentationLayer.Controllers
{
    public class BookingController : Controller
    {
        #region Fields

        IBookingService service = new BookingService();
        IUserService userService = new UserService();
        ISlotService slotService = new SlotService();
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
                    BookingId = b.BookingId,
                    Date = b.Date,
                    Capacity = b.Capacity,
                    ResourceName = b.Resource.Name,
                    User = converter.ConvertUserFromWrapper(b.User),
                    Time = b.Slot.Time,
                };
                bookings.Add(booking);
            }

            return View(bookings);
        }

        public ActionResult Timetable()
        {
            var userId = Session["UserId"].ToString();

            var data = service.GetThisWeeksBookings(new Guid(userId));
            var bookings = new List<Booking>();

            var slots = getSlots();

            foreach (wrapper.Booking b in data)
            {
                var booking = new Booking
                {
                    BookingId = b.BookingId,
                    Date = b.Date,
                    Capacity = b.Capacity,
                    ResourceName = b.Resource.Name,
                    User = converter.ConvertUserFromWrapper(b.User),
                    Time = b.Slot.Time,
                };
                bookings.Add(booking);
            }

            var timetable = new Timetable();


            //get slots

            var dataSlots = slotService.GetSlots();

            foreach(wrapper.Slot slot in dataSlots)
            {
                timetable.Slots.Add(new Slot
                    {
                        SlotId = slot.SlotId,
                        Time   = slot.Time
                    });
                timetable.MondayEntries.Add(new TimetableEntry
                {
                    Time = slot.Time,
                });

                timetable.TuesdayEntries.Add(new TimetableEntry
                {
                    Time = slot.Time,
                });

                timetable.WednesdayEntries.Add(new TimetableEntry
                {
                    Time = slot.Time,
                });

                timetable.ThursdayEntries.Add(new TimetableEntry
                {
                    Time = slot.Time,
                });

                timetable.FridayEntries.Add(new TimetableEntry
                {
                    Time = slot.Time,
                });
                    
            }
            return View(timetable);
        }

        //
        // GET: /Booking/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Booking/Create

        public ActionResult Create()
        {
            List<Slot> slots = new List<Slot>();
            var slotData = slotService.GetSlots();
            foreach (wrapper.Slot data in slotData)
            {
                slots.Add(new Slot
                {
                    Time   = data.Time,
                    SlotId = data.SlotId,
                });
            }

            var model = new Booking
            {
                Slots = slots
            };

            return View(model);
        }

        public PartialViewResult RetrieveAvailableResources(Booking booking)
        {
            var availableResources = service.GetAvailableResources(booking.Date, booking.Slot);
            var rs = new ResourceService();
            var resources = converter.ConvertResourceListFromWrapper(availableResources);
            booking.Resources = resources;

            booking.Time = slotService.GetSlot(booking.Slot).Time;

            return PartialView("_resources", booking);
        }

        [HttpPost]
        public ActionResult Book(Booking booking)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                booking.User = converter.ConvertUserFromWrapper(user);

                var convertedBooking = converter.ConvertBookingToWrapper(booking);

                service.AddBooking(convertedBooking);
            
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Create");
            }
        }


        //
        // GET: /Booking/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Booking/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Booking/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Booking/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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

        #endregion

    }
}
