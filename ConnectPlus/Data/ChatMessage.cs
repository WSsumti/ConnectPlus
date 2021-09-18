using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConnectPlus.Data
{
    public class ChatMessage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                Onchanged();
            }
        }
        string user;
        public string User
        {
            get => user;
            set
            {
                user = value;
                Onchanged();
            }
        }
        bool isOwner;
        public bool IsOwner
        {
            get => isOwner;
            set
            {
                isOwner = value;
                Onchanged();
            }
        }
    }
}
