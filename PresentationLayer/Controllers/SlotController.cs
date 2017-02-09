using DomainLayer;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class SlotController : Controller
    {
        #region Fields

            ISlotService service = new SlotService();

        #endregion
        //
        // GET: /Slot/

        public ActionResult Index()
        {
            ViewBag.Message = "Need to be able to add an employee and search for existing ones";

            var data = service.GetSlots();
            var slots = new List<Slot>();

            foreach (DomainLayer.WrapperModels.Slot s in data)
            {
                var slot = new Slot
                {
                    SlotId = s.SlotId,
                    Time   = s.Time,
                };
                slots.Add(slot);
            }

            return View(slots);
        }

        //
        // GET: /Slot/Details/5

        public ActionResult Details(Guid? slotId)
        {
            var data = service.GetSlot(slotId);

            var slot = new Slot
            {
                SlotId = data.SlotId,
                Time   = data.Time,
            };

            return View(slot);
        }

        //
        // GET: /Slot/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Slot/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var slot = new DomainLayer.WrapperModels.Slot();
                UpdateModel(slot, collection);

                service.AddSlot(slot);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Slot/Edit/5

        public ActionResult Edit(Guid? slotId)
        {
            var data = service.GetSlot(slotId);

            var slot = new Slot
            {
                SlotId = data.SlotId,
                Time   = data.Time,
            };

            return View(slot);


        }

        //
        // POST: /Slot/Edit/5

        [HttpPost]
        public ActionResult Edit(Guid? slotId, FormCollection collection)
        {
            try
            {
                var slot = new DomainLayer.WrapperModels.Slot();
                UpdateModel(slot, collection);

                service.UpdateSlot(slot);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Slot/Delete/5

        public ActionResult Delete(Guid? slotId)
        {
            var data = service.GetSlot(slotId);

            var slot = new Slot
            {
                SlotId = data.SlotId,
                Time   = data.Time,
            };

            return View(slot);
        }

        //
        // POST: /Slot/Delete/5

        [HttpPost]
        public ActionResult Delete(Guid? slotId, FormCollection collection)
        {
            try
            {
                service.DeleteSlot(slotId);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
