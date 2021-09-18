using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConnectPlus.Data
{
    public class Owner
    {
        public string Name { get; set; }
        public string Quote { get; set; }
        public List<Shoper> SubShop { get; set; }
    }
}
