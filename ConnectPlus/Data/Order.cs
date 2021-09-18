using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectPlus.Data
{
    public class Order
    {
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public int N0 { get; set; }
    }
}
