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
    public partial class Purchase : ContentPage
    {
        private User u = new User();
        string method = "";
        PurchaseAddress viewModel;
        bool t = false;
        string[] methods = { "Cash" };
        public Purchase(ShopItem item, int quan)
        {
            InitializeComponent();
            BindingContext = viewModel = new PurchaseAddress();
            Ui(item, quan);
        }
        private async void Ui(ShopItem item, int quan)
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                u = JsonConvert.DeserializeObject<User>(content);
                viewModel.ShowAddress = u.Address;
            }
            Frame fr = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            StackLayout l = new StackLayout();
            var tap = new TapGestureRecognizer();
            tap.Tapped += (object sender, EventArgs e) => ProcessAddress(sender, e, u);
            l.GestureRecognizers.Add(tap);
            Label addr = new Label()
            {
                FontSize = 13,
                Margin = new Thickness(20, 0, 0, 0),
                HorizontalOptions = LayoutOptions.Start,
            };
            addr.SetBinding(Label.TextProperty, new Binding("ShowAddress", BindingMode.Default, null, null, "Địa chỉ giao hàng: {0}"));
            l.Children.Add(addr);
            fr.Content = l;
            layout.Children.Add(fr);
            Frame itemfr = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            StackLayout il = new StackLayout();
            Label name = new Label()
            {
                Text = item.Name,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(15, 0, 0, 0),
            };
            il.Children.Add(name);
            if (DateTime.Now > item.DiscountUntil)
            {
                Label price = new Label()
                {
                    Text = item.Price.ToString(),
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                il.Children.Add(price);
            }
            else
            {
                Decimal dp = item.Price * (100 - item.DiscountPercent) / 100;
                Label price = new Label()
                {
                    Text = dp.ToString(),
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(15, 0, 0, 0),
                };
                il.Children.Add(price);
            }
            Label q = new Label()
            {
                Text = "Số lượng mua: " + quan.ToString(),
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
            };
            il.Children.Add(q);
            layout.Children.Add(il);
            StackLayout expand = new StackLayout();
            Label p = new Label()
            {
                Text = "Hình thức thanh toán: " ,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
            };
            Frame fp = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = p,
            };
            var g = new TapGestureRecognizer();
            g.Tapped += (object sender, EventArgs e) => G_Tapped(sender, e, expand);
            fp.GestureRecognizers.Add(g);
            expand.Children.Add(fp);
            layout.Children.Add(expand);
            StackLayout un = new StackLayout();
            if (DateTime.Now > item.DiscountUntil)
            {
                Decimal dp = item.Price * quan;
                Label price = new Label()
                {
                    Text = "Thanh toán: " + dp.ToString(),
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.End,
                    FontAttributes = FontAttributes.Bold,
                };
                un.Children.Add(price);
            }
            else
            {
                Decimal dp = (item.Price * (100 - item.DiscountPercent) / 100) * quan;
                Label price = new Label()
                {
                    Text = "Thanh toán: " + dp.ToString(),
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.End,
                };
                un.Children.Add(price);
            }
            Button purchase = new Button()
            {
                Text = "Thanh Toán",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.End,
            };
            purchase.Clicked += (object sender, EventArgs e) => Purchase_Clicked(sender, e, item, quan, u.Address);
            un.Children.Add(purchase);
            layout.Children.Add(un);
        }

        private async void Purchase_Clicked(object sender, EventArgs e, ShopItem s, int q, string address)
        {
            if (method != "")
            {
                if (address != "") 
                {
                    bool check;
                    using (var client = new HttpClient())
                    {
                        var content = await client.GetStringAsync(Constant.url + "purchase/check/" + s.ShopID.ToString() + "/" + s.N0.ToString());
                        check = JsonConvert.DeserializeObject<bool>(content);
                    }
                    if (check)
                    {
                        Bill bill = new Bill()
                        {
                            ShopID = s.ShopID,
                            ItemName = s.Name,
                            N0 = s.N0,
                            Quantity = q,
                            Address = address,
                            Method = method,
                        };
                        Decimal dp = s.Price * (100 - s.DiscountPercent) / 100 * q;
                        bill.Income = dp;
                        using (var client = new HttpClient())
                        {
                            var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                            User user = JsonConvert.DeserializeObject<User>(content);
                            content = await client.GetStringAsync(Constant.url + "shopers/" + s.ShopID.ToString());
                            Shoper sh = JsonConvert.DeserializeObject<Shoper>(content);
                            bill.ShopName = sh.Name;
                            List<Bill> SI = new List<Bill>(user.DeliveryItem);
                            SI.Add(bill);
                            user.DeliveryItem = SI;
                            await client.PutAsJsonAsync<User>(Constant.url + "users/" + App.Username, user);
                            await client.PostAsync(Constant.url + "reports/" + sh.Id.ToString() + "/" + dp.ToString(), new StringContent(""));
                            await client.PostAsync(Constant.url + "purchase/" + s.ShopID.ToString() + "/" + s.N0.ToString() + "/" + q.ToString(), new StringContent(""));
                        }
                        await DisplayAlert("Thông báo!", "Mua thành công", "Ok");
                        await Navigation.PushModalAsync(new DeliveryItem());
                    }
                    else
                    {
                        await DisplayAlert("Thông báo!", "Đã hết hàng", "Ok");
                    }  
                }
                else
                {
                    await DisplayAlert("Thông báo!", "Chưa thêm địa chỉ giao hàng", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Thông báo!", "Chưa chọn hình thức thanh toán", "Ok");
            }
        }

        private void G_Tapped(object sender, EventArgs e, StackLayout ex)
        {
            if (t)
            {
                for(int i =1; i<=methods.Count();++i)
                {
                    ex.Children.RemoveAt(i);
                }
                t = false;
                method = "";
            }
            else
            {
                foreach (var item in methods)
                {
                    if (item == "cash")
                    {
                        Label p = new Label()
                        {
                            Text = "Thanh toán bằng tiền mặt",
                            FontSize = 13,
                            HorizontalOptions = LayoutOptions.Start,
                            Margin = new Thickness(20, 0, 0, 0),
                        };
                        Grid.SetColumn(p, 0);
                        CheckBox check = new CheckBox()
                        {
                            IsChecked = true,
                            HorizontalOptions = LayoutOptions.End,
                        };
                        check.CheckedChanged += (object sender, CheckedChangedEventArgs e) => Check_CheckedChanged(sender, e, item);
                        Grid.SetColumn(check, 1);
                        Grid g = new Grid()
                        {
                            ColumnDefinitions =
                            {
                                new ColumnDefinition
                                {
                                    Width = new GridLength(0.5,GridUnitType.Star),
                                }
                            },
                            Children = { p, check }
                        };
                        Frame fp = new Frame()
                        {
                            BorderColor = Color.Black,
                            CornerRadius = 5,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Content = g,
                        };
                        var tap = new TapGestureRecognizer();
                        tap.Tapped += (object sender, EventArgs e) => Tap_Tapped(sender, e, check, item);
                        fp.GestureRecognizers.Add(tap);
                        ex.Children.Add(fp);
                    }
                }
                t = true;
                method = "cash";
            }

        }

        private void Check_CheckedChanged(object sender, CheckedChangedEventArgs e, string m)
        {
            if (e.Value)
            {
                method = m;
            }
            else
            {
                method = "";
            }
        }

        private void Tap_Tapped(object sender, EventArgs e, CheckBox c, string m)
        {
            if (c.IsChecked)
            {
                c.IsChecked = false;
                method = "";
            }
            else
            {
                c.IsChecked = true;
                method = m;
            }
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
                viewModel.ShowAddress = u.Address;
            }

        }
    }
}