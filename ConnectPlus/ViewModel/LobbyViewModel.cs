using ConnectPlus.Data;
using ConnectPlus.Data.DTO;
using ConnectPlus.Pages.Smaller;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConnectPlus.ViewModel
{
    public class LobbyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        ObservableCollection<RoomDTO> rooms;
        public ObservableCollection<RoomDTO> Rooms 
        { 
            get => rooms; 
            set
            {
                rooms = value;
                Onchanged();
            }
        }
        public LobbyViewModel()
        {
            rooms = new ObservableCollection<RoomDTO>();
        }
        public string UserName
        {
            get => ChatSetting.Username;
            set
            {
                if (value == UserName)
                    return;
                ChatSetting.Username = value;
                Onchanged();
            }
        }
        public string Group
        {
            get => App.Group;
            set
            {
                if (value == Group)
                    return;
                App.Group = value;
                Onchanged();
            }
        }
        public async Task GoToGroupChat(INavigation navigation, RoomDTO room)
        {
            if (string.IsNullOrWhiteSpace(room.RoomName))
                return;

            if (string.IsNullOrWhiteSpace(UserName))
                return;

            Group = room.RoomName;
            await navigation.PushModalAsync(new ChatRoom());
        }

    }
}
