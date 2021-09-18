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
    public partial class ListShopPage : ContentPage
    {
        ShopListViewModel vm;
        public ShopListViewModel VM
        {
            get => vm ?? (vm = (ShopListViewModel)BindingContext);
        }
        public ListShopPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetList();
        }
        private async Task GetList()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "shopers");
                VM.Shops = JsonConvert.DeserializeObject<List<Shoper>>(content);
            }
        }
    }
}