﻿using DomainLayer;
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
                    Capacity     = b.Capacity,
                    ResourceName = b.Resource.Name,
                    User         = converter.ConvertUserFromWrapper(b.User),
                    Time         = b.Slot.Time,
                };
                bookings.Add(booking);
            }

            var slots = slotService.GetSlots();

            var timetable = new List<TimetableEntry>();

            #region Add entries to timetable

            foreach (wrapper.Slot slot in slots)
            {
                switch (slot.Time)
                {
                    case "09:00 - 10:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "09:00 - 10:00",
                                MondayResource    = "---",
                                TuesdayResource   = "---",
                                WednesdayResource = "---",
                            });
                            break;
                        }
                    case "10:00 - 11:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "10:00 - 11:00",
                                MondayResource    = "---",
                                TuesdayResource   = "---",
                                WednesdayResource = "---",
                            });
                            break;
                        }
                    case "11:00 - 12:00":
                        {
                            timetable.Add(new TimetableEntry
                            {
                                Time              = "11:00 - 12:00",
                                MondayResource    = "---",
                                TuesdayResource   = "---",
                                WednesdayResource = "---",
                            });
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            #endregion

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
            var rs                 = new ResourceService();
            var resources          = converter.ConvertResourceListFromWrapper(availableResources);
            booking.Resources      = resources;

            booking.Time = slotService.GetSlot(booking.Slot).Time;

            return PartialView("_resources", booking);
        }

        [HttpPost]
        public ActionResult Book(Booking booking)
        {
            try
            {
                var userId = Session["UserId"].ToString();
                var user   = userService.GetUser(new Guid(userId));

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
