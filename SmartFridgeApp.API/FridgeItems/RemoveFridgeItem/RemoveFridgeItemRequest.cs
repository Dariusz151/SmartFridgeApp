using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFridgeApp.API.FridgeItems.RemoveFridgeItem
{
    public class RemoveFridgeItemRequest
    {
        public Guid FridgeItemId { get; set; }
        public Guid UserId { get; set; }
    }
}
