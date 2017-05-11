using DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
namespace DomainLayer
{
    public interface ISlotService
    {
        /// <summary>
        /// Gets all of the slots in the database
        /// </summary>
        /// <returns>Returns a list of slots</returns>
        List<Slot> GetSlots();

        /// <summary>
        /// Gets the slot using the id
        /// </summary>
        /// <returns>Returns the slot</returns>
        Slot GetSlot(Guid? slotId);

        /// <summary>
        /// Adds a slot to the database
        /// </summary>
        /// <param name="slot">The new slot to be added</param>
        void AddSlot(Slot slot);

        /// <summary>
        /// Updates an existing slot
        /// </summary>
        /// <param name="data">The new slot details</param>
        void UpdateSlot(Slot data);

        /// <summary>
        /// Deletes an existing slot from the database
        /// </summary>
        /// <param name="data">The slot to be deleted</param>
        void DeleteSlot(Guid? slotId);
    }
}
