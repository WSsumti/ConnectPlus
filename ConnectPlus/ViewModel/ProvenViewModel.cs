using ConnectPlus.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConnectPlus.ViewModel
{
    public class ProvenViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        List<Request> requests;
        public List<Request> Requests
        {
            get => requests;
            set
            {
                requests = value;
                Onchanged();
            }
        }
    }
}
