using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFridgeApp.Domain.Shared;

namespace SmartFridgeApp.API.FridgeItems
{
    public class AmountValueDto
    {
        public float Value { get; set; }
        public Unit Unit { get; set; }
    }
}
