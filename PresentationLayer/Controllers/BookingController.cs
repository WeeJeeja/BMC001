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
                    //Resource = converter.ConvertResourceFromWrapper(b.Resource),
                    User = converter.ConvertUserFromWrapper(b.User),
                    //Slot = converter.ConvertSlotFromWrapper(b.Slot),
                };
                bookings.Add(booking);
            }

            return View(bookings);
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
                    Time  = data.Time,
                    SlotId = data.SlotId,
                });
            }

            var userId = Session["UserId"].ToString();
            var user   = userService.GetUser(new Guid(userId));

            var model = new Booking
            {
                User  = converter.ConvertUserFromWrapper(user),
                Slots = slots
            };

            return View(model);
        }

        //
        // POST: /Booking/Create

        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            try
            {
                //var resourceData = service.GetAvailableResources(booking.Date, booking.Slot);
                ///*foreach (wrapper.Resource data in resourceData)
                //{
                //   * booking.Resources.Add(new Resource
                //        {
                //            ResourceId  = data.ResourceId,
                //            Name        = data.Name,
                //            Description = data.Description,
                //            Capacity    = data.Capacity,
                //            Category    = data.Category,
                //        });
                //}*/

                //booking.Slots = getSlots();

                return View(booking);
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Booking/StepTwo

        public ActionResult StepTwo(Booking model)
        {
            List<SelectListItem> resources = new List<SelectListItem>();

            //add user to booking
            //check user does not already have a booking for this date/time
            //add booking to db


            return View(model);
        }

        //
        // POST: /Booking/StepTwo

        [HttpPost]
        public ActionResult StepTwo(FormCollection collection)
        {
            try
            {
                var booking = new DomainLayer.WrapperModels.Booking();
                UpdateModel(booking, collection);

                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                booking.User = user;


                return RedirectToAction("StepTwo");
            }
            catch
            {
                return View();
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
