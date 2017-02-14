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
                    Date      = b.Date,
                    Capacity  = b.Capacity,
                    Resource  = converter.ConvertResourceFromWrapper(b.Resource),
                    User      = converter.ConvertUserFromWrapper(b.User),
                    Slot      = converter.ConvertSlotFromWrapper(b.Slot),
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

            var model = new Booking
            {
                Slots = slots,
            };
            return View(model);
        }

        //
        // POST: /Booking/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var booking = new DomainLayer.WrapperModels.Booking();
                UpdateModel(booking, collection);

                var userId = Session["UserId"].ToString();
                var user = userService.GetUser(new Guid(userId));

                booking.User = user;

                //service.AddBooking(booking);

                return RedirectToAction("Index");
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
    }
}
