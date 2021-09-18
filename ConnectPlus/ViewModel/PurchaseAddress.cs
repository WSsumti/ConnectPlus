using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConnectPlus.ViewModel
{
    public class PurchaseAddress : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        string address;
        public string ShowAddress
        {
            get => address;
            set
            {
                address = value;
                Onchanged();
            }
        }

    }
}
