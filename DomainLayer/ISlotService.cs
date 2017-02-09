using System;
namespace DomainLayer
{
    public interface ISlotService
    {
        void AddSlot(DomainLayer.WrapperModels.Slot slot);
        void DeleteSlot(Guid? slotId);
        DomainLayer.WrapperModels.Slot GetSlot(Guid? slotId);
        System.Collections.Generic.List<DomainLayer.WrapperModels.Slot> GetSlots();
        void UpdateSlot(DomainLayer.WrapperModels.Slot data);
    }
}
