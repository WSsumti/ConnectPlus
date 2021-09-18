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
    public partial class Show : ContentPage
    {
        List<Image> imgs = new List<Image>();
        public Show(ShopItem item, int pos)
        {
            InitializeComponent();
            Ui(item, pos);
            BindingContext = this;
        }

        public Show(ShopItem item)
        {
            InitializeComponent();
        }

        private async void Ui(ShopItem item, int pos)
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "shopers/" + item.ShopID);
                Shoper s = JsonConvert.DeserializeObject<Shoper>(content);
                if (DateTime.Now > item.DiscountUntil)
                {
                    item.IsDiscount = false;
                    item.DiscountPercent = 0;
                }
                item.Access++;
                s.Packages[pos] = item;
                await client.PutAsJsonAsync<Shoper>(Constant.url + "shopers/" + item.ShopID, s);
            }
            StackLayout st = new StackLayout();
            CarouselView images = new CarouselView();
            StackLayout stla = new StackLayout()
            {
                HeightRequest = 330,
            };
            using (var client = new HttpClient())
            {
                foreach (var img in item.IntroImages)
                {
                    var b = await client.GetByteArrayAsync(Constant.url + "files/" + img);
                    var ms = new MemoryStream(b);

                    Image i = new Image()
                    {
                        Source = ImageSource.FromStream(() => ms),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };
                    imgs.Add(i);
                }
            }
            images.Loop = false;
            images.ItemsSource = imgs;
            images.ItemTemplate = new DataTemplate(() =>
            {
                Image image = new Image()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFill,
                };
                image.SetBinding(Image.SourceProperty, "Source");
                StackLayout root = new StackLayout()
                {
                    Children = { image }
                };
                return root;
            });
            stla.Children.Add(images);
            st.Children.Add(stla);
            Label name = new Label()
            {
                Text = "Tên sản phẩm: " + item.Name,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
            };
            st.Children.Add(name);

            Label price = new Label()
            {
                Text = "Giá: " + item.Price.ToString(),
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
            };
            st.Children.Add(price);

            if (item.IsDiscount)
            {
                price.TextColor = Color.Gray;
                Decimal dp = item.Price * (100 - item.DiscountPercent) / 100;
                Label dis = new Label()
                {
                    Text = "Giá giảm giá: " + dp.ToString(),
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(20, 0, 0, 0),
                };
                st.Children.Add(dis);
            };

            Label q = new Label()
            {
                Text = "Số lượng còn trong kho: " + item.QuantityInShop.ToString(),
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
            };
            st.Children.Add(q);

            Label des = new Label()
            {
                Text = item.Description,
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
            };
            st.Children.Add(des);
            Button purchase = new Button()
            {
                Text = "Thêm vào giỏ hàng",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.End,
            };
            purchase.Clicked += (object sender, EventArgs e) => Purchase_Clicked(sender, e, item);
            st.Children.Add(purchase);
            scroll.Content = st;
        }

        private async void Purchase_Clicked(object sender, EventArgs e, ShopItem s)
        {
            App.Orders.Add(new Order
            {
                N0 = s.N0,
                Name = s.Name,
                Price = s.Price,
                ShopId = s.ShopID,
            });
            await DisplayAlert("Thông báo", "Đã thêm vào giỏ hàng", "Ok");
        }
    }
}