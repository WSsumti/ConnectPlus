using ConnectPlus.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;

namespace ConnectPlus.Pages.Smaller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemforPr : ContentPage
    {
        public int no { get; set; }
        public List<ShopItem> list { get; set; }
        public ItemforPr()
        {
            InitializeComponent();
            BindingContext = this;
            Ui();
        }
        private async void Ui()
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "shopers/" + App.Idd);
                Shoper s = JsonConvert.DeserializeObject<Shoper>(content);
                list = s.Packages;
                view.SetBinding(CollectionView.ItemsSourceProperty, "list");
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var fr = (MyFrame)sender;
            fr.BackgroundColor = Color.Aquamarine;
            no = fr.ID;
            await Navigation.PopModalAsync();
        }
        
    }
    public partial class MyFrame : Frame
    {
        public static readonly BindableProperty IDProperty = BindableProperty.Create(
                                    nameof(ID),
                                    typeof(int),
                                    typeof(MyFrame),
                                    -1);
        public int ID
        {
            get => (int)GetValue(IDProperty);
            set => SetValue(IDProperty, value);
        }
    }
}