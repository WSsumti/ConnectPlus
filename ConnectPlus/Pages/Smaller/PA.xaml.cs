using ConnectPlus.Data;
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
    public partial class PA : ContentPage
    {
        public PA(User u)
        {
            InitializeComponent();
            if (u.Address == "")
            {
                Ui(u);
            }
            else
            {
                Uii(u);
            }
        }
        private void Ui(User u)
        {
            Label i = new Label()
            {
                Text = "Địa chỉ giao hàng ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center,
            };
            layout.Children.Add(i);
            Frame fr = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            Label intr = new Label()
            {
                Text = "Nhập địa chỉ giao hàng: ",
                FontSize = 13,
                Margin = new Thickness(20, 0, 0, 0),
                HorizontalOptions = LayoutOptions.Start,
            };
            Entry en = new Entry()
            {
                FontSize = 13,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            StackLayout l = new StackLayout() { Children = { intr, en } };
            fr.Content = l;
            layout.Children.Add(fr);
            Button save = new Button()
            {
                Text = "Save",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.End,
            };
            save.Clicked += (object sender, EventArgs e) => Save_Clicked(sender,e,u,en.Text);
            layout.Children.Add(save);
        }
        private void  Uii(User u)
        {
            Label i = new Label()
            {
                Text = "Đổi địa chỉ giao hàng ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Center,
            };
            layout.Children.Add(i);
            Label ii = new Label()
            {
                Text = "Danh sách địa chỉ: ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(15, 0, 0, 0),
            };
            layout.Children.Add(ii);
            foreach (var a in u.SubAddress)
            {
                Frame fr = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 5,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                Label intr = new Label()
                {
                    Text = a,
                    FontSize = 13,
                    Margin = new Thickness(20, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                Button take = new Button()
                {
                    Text = "Chọn",
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.End,
                };
                take.Clicked += (object sender, EventArgs e) => Take_Clicked(sender, e, a, u);
                StackLayout l = new StackLayout() { Children = { intr, take } };
                fr.Content = l;
                layout.Children.Add(fr);
            }
            Frame _fr = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            Label _intr = new Label()
            {
                Text = "Nhập địa chỉ giao hàng mới: ",
                FontSize = 13,
                Margin = new Thickness(20, 0, 0, 0),
                HorizontalOptions = LayoutOptions.Start,
            };
            Entry _en = new Entry()
            {
                FontSize = 13,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            StackLayout _l = new StackLayout() { Children = { _intr, _en } };
            _fr.Content = _l;
            layout.Children.Add(_fr);
            Button _save = new Button()
            {
                Text = "Save",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.End,
            };
            _save.Clicked += (object sender, EventArgs e) => Save_Clicked(sender, e, u, _en.Text);
            layout.Children.Add(_save);
        }

        private async void Take_Clicked(object sender, EventArgs e, string ad, User u)
        {
            using (var client = new HttpClient())
            {
                u.Address = ad;
                await client.PutAsJsonAsync<User>(Constant.url + "users/" + App.Username, u);
            }
            await Navigation.PopModalAsync();
        }

        private async void Save_Clicked(object sender, EventArgs e, User u, string ad)
        {
            u.Address = ad;
            List<string> newAd = new List<string>(u.SubAddress);
            newAd.Add(ad);
            u.SubAddress = newAd;
            using (var client = new HttpClient())
            {
                await client.PutAsJsonAsync<User>(Constant.url + "users/" + App.Username, u);
            }
            await DisplayAlert("Thông báo: ", "Tạo địa chỉ thành công", "Ok");
            await Navigation.PopModalAsync();
        }
    }
}