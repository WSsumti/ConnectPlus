using ConnectPlus.Data;
using ConnectPlus.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Smaller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        CartModel viewmodel;
        private User u = new User();
        public CartPage()
        {
            InitializeComponent();
            var tap = new TapGestureRecognizer();
            tap.Tapped += (object sender, EventArgs e) => ProcessAddress(sender, e, u);
            l.GestureRecognizers.Add(tap);
            BindingContext = viewmodel = new CartModel();
            viewmodel.Orders = App.Orders;
            Load();
        }
        private async void Load()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                u = JsonConvert.DeserializeObject<User>(content);
                viewmodel.Address = u.Address;
            }
        }

        private void Lis_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void ProcessAddress(object sender, EventArgs e, User u)
        {
            await Navigation.PushModalAsync(new PA(u));
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                u = JsonConvert.DeserializeObject<User>(content);
                viewmodel.Address = u.Address;
            }
            viewmodel.Orders = App.Orders;
        }
    }
}