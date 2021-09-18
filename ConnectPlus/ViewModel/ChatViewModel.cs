using ConnectPlus.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ConnectPlus.ViewModel
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        HubConnection hubConnection;
        public ChatMessage ChatMessage { get; }
        public ObservableRangeCollection<ChatMessage> Messages { get; }
        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        Random random;
        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    isConnected = value;
                    Onchanged();
                });
            }
        }
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                Onchanged();
            }
        }
        public ChatViewModel()
        {
            ChatMessage = new ChatMessage();
            Messages = new ObservableRangeCollection<ChatMessage>();
            GetOlder();
            
            SendMessageCommand = new Command(async () => await SendMessage());
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
            hubConnection = new HubConnectionBuilder().WithUrl(Constant.Hurl).Build();
            hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                await Task.Delay(random.Next(0, 5) * 1000);
                await Connect();
            };
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                if (user == ChatSetting.Username)
                    SendLocalMessage(message, true);
                else
                {
                    SendLocalMessage(message, false);
                }
            });
            hubConnection.On<string>("EnteredOrLeft", (message) =>
            {
                SendLocalMessage(message, true);
            });
        }
        private async void GetOlder()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "roomchats/" + App.Group);
                var r = JsonConvert.DeserializeObject<RoomChat>(content);
                if (r.User1.Username == ChatSetting.Username)
                {
                    foreach (var item in r.User1.History)
                    {
                        Messages.Add(item);
                    }
                }
                else
                {
                    foreach (var item in r.User2.History)
                    {
                        Messages.Add(item);
                    }
                }
            }
        }
        private void SendLocalMessage(string message, bool isowner)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new ChatMessage
                {
                    Message = message,
                    IsOwner = isowner,
                });
            });
        }
        async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                await hubConnection.InvokeAsync("SendMessageGroup", App.Group, ChatSetting.Username, ChatMessage.Message);
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync(Constant.url + "roomchats/" + App.Group);
                    var r = JsonConvert.DeserializeObject<RoomChat>(content);
                    if (r.User1.Username == ChatSetting.Username)
                    {
                        var mm = r.User1.History;
                        mm.Add(new ChatMessage
                        {
                            Message = ChatMessage.Message,
                            IsOwner = true,
                        });
                        r.User1.History = mm;
                        var mmm = r.User2.History;
                        mmm.Add(new ChatMessage
                        {
                            Message = ChatMessage.Message,
                            IsOwner = false,
                        });
                        r.User2.History = mmm;
                        await client.PutAsJsonAsync<RoomChat>(Constant.url + "roomchats/" + App.Group, r);
                    }
                    else
                    {
                        var mm = r.User2.History;
                        mm.Add(new ChatMessage
                        {
                            Message = ChatMessage.Message,
                            IsOwner = true,
                        });
                        r.User2.History = mm;
                        var mmm = r.User1.History;
                        mmm.Add(new ChatMessage
                        {
                            Message = ChatMessage.Message,
                            IsOwner = false,
                        });
                        r.User1.History = mmm;
                        await client.PutAsJsonAsync<RoomChat>(Constant.url + "roomchats/" + App.Group, r);
                    }
                }
                ChatMessage.Message = "";
            }
            catch (Exception)
            {
                SendLocalMessage("Send Error", true);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                await hubConnection.SendAsync("AddToGroup", App.Group, ChatSetting.Username);
                IsConnected = true;
                SendLocalMessage("Connected...", true);
            }
            catch (Exception)
            {
                SendLocalMessage("Connect Error", true);
            }
        }
        async Task Disconnect()
        {
            if (!IsConnected)
                return;
            await hubConnection.SendAsync("RemoveFromGroup", App.Group, ChatSetting.Username);
            await hubConnection.StopAsync();
            IsConnected = false;
            SendLocalMessage("Disconnected", true);
        }
    }
}
