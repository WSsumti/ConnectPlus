using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConnectPlus.ViewModel
{
    public class DeliveryModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        string _title;
        public string ShowTitle
        {
            get => _title;
            set
            {
                _title = value;
                Onchanged();
            }
        }
    }
}
