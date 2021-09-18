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
    public partial class DeliveryItem : ContentPage
    {
        private User user = new User();
        DeliveryModel view;
        List<Bill> bills = new List<Bill>();
        public DeliveryItem()
        {
            InitializeComponent();
            BindingContext = view = new DeliveryModel();
            Ui();
        }
        public async void Ui()
        {
            Button deli = new Button()
            {
                Text = "Đang xử lý và giao hàng",
                FontSize = 12,
            };
            deli.Clicked += Deli_Clicked;
            StackLayout s = new StackLayout() { Children = { deli } };
            Frame delifr = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 20,
                Content = s,
                HorizontalOptions = LayoutOptions.Start,
            };
            Grid.SetColumn(delifr, 0);
            gr.Children.Add(delifr);

            Button deled = new Button()
            {
                Text = "Đã giao hàng",
                FontSize = 12,
            };
            deled.Clicked += Deled_Clicked;
            StackLayout ss = new StackLayout() { Children = { deled } };
            Frame deledfr = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 20,
                Content = ss,
                HorizontalOptions = LayoutOptions.End,
            };
            Grid.SetColumn(deledfr, 1);
            gr.Children.Add(deledfr);
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                user = JsonConvert.DeserializeObject<User>(content);
            }
            foreach (var item in user.DeliveryItem)
            {
                if (!item.Delivered)
                {
                    bills.Add(item);
                }
            }
            foreach (var bill in bills)
            {
                Frame fr = new Frame()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    CornerRadius = 10,
                    BorderColor = Color.Black,
                };
                Label deal = new Label()
                {
                    Text = "Đơn hàng: ",
                    FontSize = 13,
                    Margin = new Thickness(20, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label name = new Label()
                {
                    Text = "Tên sản phẩm: " + bill.ItemName,
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label price = new Label()
                {
                    Text = "Giá sản phẩm: " + bill.Income.ToString(),
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label quan = new Label()
                {
                    Text = "Số lượng sản phẩm: " + bill.Quantity.ToString(),
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label address = new Label()
                {
                    Text = "Địa chỉ giao về: " + bill.Address,
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                StackLayout l = new StackLayout() { Children = { name, price, quan, address } };
                fr.Content = l;
                ap.Children.Add(fr);
            }
        }

        private async void Deled_Clicked(object sender, EventArgs e)
        {
            view.ShowTitle = "Đã giao hàng";
            ap.Children.Clear();
            bills.Clear();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                user = JsonConvert.DeserializeObject<User>(content);
            }
            foreach (var item in user.DeliveryItem)
            {
                if (item.Delivered)
                {
                    bills.Add(item);
                }
            }
            foreach (var bill in bills)
            {
                Frame fr = new Frame()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    CornerRadius = 10,
                    BorderColor = Color.Black,
                };
                Label deal = new Label()
                {
                    Text = "Đơn hàng: ",
                    FontSize = 13,
                    Margin = new Thickness(20, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label name = new Label()
                {
                    Text = "Tên sản phẩm: " + bill.ItemName,
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label price = new Label()
                {
                    Text = "Giá sản phẩm: " + bill.Income.ToString(),
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label quan = new Label()
                {
                    Text = "Số lượng sản phẩm: " + bill.Quantity.ToString(),
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label address = new Label()
                {
                    Text = "Địa chỉ giao về: " + bill.Address,
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                StackLayout l = new StackLayout() { Children = { deal, name, price, quan, address } };
                fr.Content = l;
                ap.Children.Add(fr);
            }
        }
        private async void Deli_Clicked(object sender, EventArgs e)
        {
            view.ShowTitle = "Đang xử lý và giao hàng";
            ap.Children.Clear();
            bills.Clear();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                user = JsonConvert.DeserializeObject<User>(content);
            }
            foreach (var item in user.DeliveryItem)
            {
                if (!item.Delivered)
                {
                    bills.Add(item);
                }
            }
            foreach (var bill in bills)
            {
                Frame fr = new Frame()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    CornerRadius = 10,
                    BorderColor = Color.Black,
                };
                Label deal = new Label()
                {
                    Text = "Đơn hàng: ",
                    FontSize = 13,
                    Margin = new Thickness(20, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label name = new Label()
                {
                    Text = "Tên sản phẩm: " + bill.ItemName,
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label price = new Label()
                {
                    Text = "Giá sản phẩm: " + bill.Income.ToString(),
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label quan = new Label()
                {
                    Text = "Số lượng sản phẩm: " + bill.Quantity.ToString(),
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Label address = new Label()
                {
                    Text = "Địa chỉ giao về: " + bill.Address,
                    FontSize = 13,
                    Margin = new Thickness(10, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                StackLayout l = new StackLayout() { Children = { deal, name, price, quan, address } };
                fr.Content = l;
                ap.Children.Add(fr);
            }
        }
    }
}