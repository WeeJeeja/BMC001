using DataLayer;
using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer
{
    public class SlotService : ISlotService
    {
        /// <summary>
        /// Gets all of the slots in the database
        /// </summary>
        /// <returns>Returns a list of slots</returns>
        public List<Slot> GetSlots()
        {
            var db = new ReScrumEntities();

            var data = db.Slots.Where(s => s.CancellationDate == null).ToList();
            var slots = new List<Slot>();

            foreach (DataLayer.Models.Slot s in data)
            {
                var slot = new Slot
                {
                    SlotId = s.SlotId,
                    Time   = s.Time
                };
                slots.Add(slot);
            }

            return slots;
        }

        /// <summary>
        /// Gets the slot using the id
        /// </summary>
        /// <returns>Returns the slot</returns>
        public Slot GetSlot(Guid? slotId)
        {
            var db = new ReScrumEntities();

            var data = db.Slots.Where(s => s.SlotId == slotId).FirstOrDefault();
            
            var slot = new Slot
            {
                SlotId     = data.SlotId,
                Time       = data.Time,
                StartTime  = data.StartTime,
                EndTime    = data.EndTime,
            };
            return slot;
        }

        /// <summary>
        /// Adds a slot to the database
        /// </summary>
        /// <param name="slot">The new slot to be added</param>
        public void AddSlot(Slot slot)
        {
            var db = new ReScrumEntities();

            var newslot = new DataLayer.Models.Slot
            {
                SlotId = slot.SlotId,
                Time   = slot.Time
            };

            db.Slots.Add(newslot);

            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing slot
        /// </summary>
        /// <param name="data">The new slot details</param>
        public void UpdateSlot(Slot data)
        {
            var db = new ReScrumEntities();

            var slot = db.Slots.Where(u => u.SlotId == data.SlotId).FirstOrDefault();

            slot.Time = data.Time;

            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing slot from the database
        /// </summary>
        /// <param name="data">The slot to be deleted</param>
        public void DeleteSlot(Guid? slotId)
        {
            var db = new ReScrumEntities();

            var slot = db.Slots.Where(u => u.SlotId == slotId).FirstOrDefault();

            slot.CancellationDate = DateTime.Today;

            db.SaveChanges();
        }
    }
}
