﻿using DomainLayer;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class BookingController : Controller
    {
        #region Fields

        IBookingService service = new BookingService();

        #endregion

        //
        // GET: /Booking/

        public ActionResult Index()
        {
            ViewBag.Message = "Need to be able to add, edit and delete bookings";

            var userId = Session["UserId"].ToString();

            var data = service.GetThisWeeksBookings(new Guid(userId));
            var bookings = new List<Booking>();

            foreach (DomainLayer.WrapperModels.Booking b in data)
            {
                var slot = new Booking
                {
                    BookingId = b.BookingId,
                    Date = b.Date,
                    Capacity = b.Capacity,
                    Resource = b.Resource,
                    User = b.User,
                    Slot = b.Slot,
                };
                slots.Add(slot);
            }

            return View(slots);
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
            return View();
        }

        //
        // POST: /Booking/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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
