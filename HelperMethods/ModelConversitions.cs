using wrapper = DomainLayer.WrapperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data = DataLayer.Models;

namespace HelperMethods
{
    public class ModelConversitions
    {
        public wrapper.Slot ConvertDataSlotToWrapper(data.Slot entry)
        {
            var slot = new wrapper.Slot
            {
                SlotId = entry.SlotId,
                Time   = entry.Time,
            };

            return slot;
        }

        public wrapper.Resource ConvertDataResourceToWrapper(data.Resource entry)
        {
            var resource = new wrapper.Resource
            {
                ResourceId  = entry.ResourceId,
                Name        = entry.Name,
                Description = entry.Description,
                Category    = entry.Category,
                Capacity    = entry.Capacity,
                Location    = entry.Location,
            };

            return resource;
        }
    }
}
