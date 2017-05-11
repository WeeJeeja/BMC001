using DomainLayer;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    public class SlotController : Controller
    {
        #region Fields

            ISlotService service = new SlotService();

        #endregion
        
        /// <summary>
        /// Gets the list of active slots
        /// </summary>
        /// <returns>Slot/Index</returns>
        public ActionResult Index()
        {
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

        /// <summary>
        /// Gets the slot details
        /// </summary>
        /// <param name="slotId">The slot</param>
        /// <returns>Slot/Details/SlotId</returns>
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

        /// <summary>
        /// Gets the create slot page
        /// </summary>
        /// <returns>Slot/Create</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Posts the new slot data
        /// </summary>
        /// <param name="collection">The for slot data</param>
        /// <returns>Slot/Index</returns>
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

        /// <summary>
        /// Gtes the slot edit page
        /// </summary>
        /// <param name="slotId">The slot</param>
        /// <returns>Slot/Edit/SlotId</returns>
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

        /// <summary>
        /// Posts the slot updated data
        /// </summary>
        /// <param name="slotId">The slot Id</param>
        /// <param name="collection">Updated slot data</param>
        /// <returns>Slot/Index</returns>
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

        /// <summary>
        /// Gets the slot delete page
        /// </summary>
        /// <param name="slotId">The slot Id</param>
        /// <returns>Slot/Delete/SlotId</returns>
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

        /// <summary>
        /// Posts the slot to be deleted
        /// </summary>
        /// <param name="slotId">The slot Id</param>
        /// <param name="collection">The slot data</param>
        /// <returns>Slot/Index</returns>
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
