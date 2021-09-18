using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConnectPlus.Data;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Input;
using Xamarin.Forms;
using ConnectPlus.Pages.Smaller;
using ConnectPlus.Data.DTO;
using System.IO;

namespace ConnectPlus.ViewModel
{
    public class SubHomeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void Onchanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        List<ItemDTO> items;
        public List<ItemDTO> Items
        {
            get => items;
            set
            {
                items = value;
                Onchanged();
            }
        }
        
        ItemDTO selected;
        public ItemDTO Selected
        {
            get => selected;
            set
            {
                selected = value;
                Onchanged();
            }
        }
        public ICommand SelectionCommand => new Command(ShowItem);
        private async void ShowItem()
        {
            if (Selected != null)
            {
                ShopItem s = new ShopItem();
                using (var client = new HttpClient())
                {
                    var cont = await client.GetStringAsync(Constant.url + "shopers/" + Selected.ShopID.ToString());
                    s = JsonConvert.DeserializeObject<Shoper>(cont).Packages[Selected.N0];
                }
                var navigation = Application.Current.MainPage as NavigationPage;
                await navigation.PushAsync(new Show(s, s.N0), true);
            }
        }
    }
}
