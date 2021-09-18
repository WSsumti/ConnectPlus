using ConnectPlus.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConnectPlus.ViewModel
{
    public class ShopListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        List<Shoper> shops;
        public List<Shoper> Shops
        {
            get => shops;
            set
            {
                shops = value;
                Onchanged();
            }
        }
    }
}
