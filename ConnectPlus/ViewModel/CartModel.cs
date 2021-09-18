using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConnectPlus.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ConnectPlus.ViewModel
{
    public class CartModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                Onchanged();
            }
        }
        ObservableCollection<Order> _Orders;
        public ObservableCollection<Order> Orders
        { 
            get => _Orders; 
            set 
            { 
                _Orders = value; 
                Onchanged(); 
            } 
        }
    }
}
