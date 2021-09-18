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
    public partial class ProvenPage : ContentPage
    {
        ProvenViewModel vm;
        public ProvenViewModel VM
        {
            get => vm ?? (vm = (ProvenViewModel)BindingContext);
        }
        public ProvenPage()
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
                var content = await client.GetStringAsync(Constant.url + "pendings/getlist");
                VM.Requests = JsonConvert.DeserializeObject<List<Request>>(content);
            }
        }
    }
}