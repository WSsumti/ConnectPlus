using ConnectPlus.Data;
using ConnectPlus.Data.DTO;
using ConnectPlus.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Smaller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatLobby : ContentPage
    {
        LobbyViewModel vm;
        public LobbyViewModel VM
        {
            get => vm ?? (vm = (LobbyViewModel)BindingContext);
        }
        public ChatLobby()
        {
            InitializeComponent();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            await VM.GoToGroupChat(Navigation, e.SelectedItem as RoomDTO);
            ((ListView)sender).SelectedItem = null;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            VM.Rooms = new ObservableCollection<RoomDTO>();
            using (var client = new HttpClient())
            {
                if (ChatSetting.Username == App.Username)
                {
                    var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                    var u = JsonConvert.DeserializeObject<User>(content);
                    foreach (var item in u.KeyRoom)
                    {
                        VM.Rooms.Add(new RoomDTO()
                        {
                            RoomName = item,
                            RoomDisplay = item.Replace(ChatSetting.Username, "").Trim('-'),
                        });
                    } 
                }
                else
                {
                    var content = await client.GetStringAsync(Constant.url + "shopers/" + App.Idd);
                    var sh = JsonConvert.DeserializeObject<Shoper>(content);
                    foreach (var item in sh.KeyRoom)
                    {
                        VM.Rooms.Add(new RoomDTO()
                        {
                            RoomName = item,
                            RoomDisplay = item.Replace(ChatSetting.Username, "").Trim('-'),
                        });
                    }
                }
            }
        }
    }
}