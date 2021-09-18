using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConnectPlus.Data.DTO
{
    public class ItemDTO
    {
        public Guid ShopID { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
        public Decimal Price { get; set; }
        public int N0 { get; set; }
    }
}
