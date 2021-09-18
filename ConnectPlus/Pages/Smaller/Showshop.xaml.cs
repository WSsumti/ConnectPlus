using ConnectPlus.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Smaller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Showshop : ContentPage
    {
        public Showshop(Shoper shop)
        {
            InitializeComponent();
            Ui(shop);
        }

        private async void Ui(Shoper shop)
        {
            StackLayout l = new StackLayout();
            ImageButton ava = new ImageButton();
            using (var client = new HttpClient())
            {
                var b = await client.GetByteArrayAsync(Constant.url + "files/" + shop.Avatar);
                var ms = new MemoryStream(b);
                ava.Source = ImageSource.FromStream(() => ms);
                ava.HeightRequest = 100;
                ava.WidthRequest = 100;
                ava.HorizontalOptions = LayoutOptions.Start;
                ava.BackgroundColor = Color.White;
            }
            Grid grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition()
                    {
                        Width = new GridLength(0.4, GridUnitType.Star),
                    }
                },
                RowDefinitions =
                {
                    new RowDefinition()
                    {
                        Height = new GridLength(0.5,GridUnitType.Star),
                    }
                },
            };
            Button name = new Button()
            {
                Text = shop.Name,
                FontSize = 12,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = Color.White,
            };
            Grid.SetColumn(ava, 0);
            Grid.SetRowSpan(ava, 2);
            Grid.SetColumn(name, 1);
            Grid.SetRow(name, 0);
            grid.Children.Add(name);
            grid.Children.Add(ava);
            l.Children.Add(grid);
            Button follow = new Button()
            {
                Text = "Theo dõi",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.End,
            };
            follow.Clicked += (object o , EventArgs e) =>Follow_Clicked(o,e,shop);
            l.Children.Add(follow);
            layout.Children.Add(l);
            Button chat = new Button()
            {
                Text = "Liên hệ shop",
                FontSize = 13,
            };
            chat.Clicked += (object sender, EventArgs e) => Chat_Clicked(sender, e, shop);
            layout.Children.Add(chat);
        }

        private async void Chat_Clicked(object sender, EventArgs e, Shoper shop)
        {
            string key = App.Username + "-" + shop.Name;
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "roomchats/" + key);
                if (JsonConvert.DeserializeObject<RoomChat>(content) == null) 
                {
                    RoomChat room = new RoomChat()
                    {
                        Key = App.Username + "-" + shop.Name,
                        User1 = new ChatData()
                        {   
                            Username = App.Username,
                            History = new List<ChatMessage>(),
                        },
                        User2 = new ChatData()
                        {
                            Username = shop.Name,
                            History = new List<ChatMessage>(),
                        },
                    };
                    var a = shop.KeyRoom;
                    a.Add(room.Key);
                    shop.KeyRoom = a;
                    await client.PutAsJsonAsync<Shoper>(Constant.url + "shopers/" + shop.Id.ToString(), shop);
                    var con = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                    var u = JsonConvert.DeserializeObject<User>(con);
                    var t = u.KeyRoom;
                    t.Add(room.Key);
                    u.KeyRoom = t;
                    await client.PutAsJsonAsync<User>(Constant.url + "users/" + App.Username, u);
                    await client.PostAsJsonAsync<RoomChat>(Constant.url + "roomchats", room);
                }
            }
            ChatSetting.Username = App.Username;
            await Navigation.PushModalAsync(new ChatLobby());
        }

        private async void Follow_Clicked(object sender, EventArgs e, Shoper f)
        {
            List<Shoper> s = new List<Shoper>();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                User u = JsonConvert.DeserializeObject<User>(content);
                foreach (var item in u.FollowingShops)
                {
                    s.Add(item);
                }
                s.Add(f);
                u.FollowingShops = s;
                await client.PutAsJsonAsync<User>(Constant.url + "users/" + App.Username, u);
                f.Information.nFollowers++;
                await client.PutAsJsonAsync<Shoper>(Constant.url + "shopers/" + f.Id, f);
            }
            await DisplayAlert("Thông báo:", "Theo dõi thành công!", "Ok");
        }
    }
}