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
    public partial class SearchResult : ContentPage
    {
        private List<Searche> items = new List<Searche>();
        List<Image> images = new List<Image>();
        List<Shoper> ss = new List<Shoper>();
        List<Image> bts = new List<Image>();  
        public SearchResult(string keyword)
        {
            InitializeComponent();
            naviText.Text = keyword;
            Ui(keyword, 0);
        }

        private async void Ui(string keyword, int pos)
        {
            items.Clear();
            images.Clear();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "searches/allsearches/" + keyword + "/" + pos.ToString());
                items = JsonConvert.DeserializeObject<List<Searche>>(content);
                List<Searche> remove = new List<Searche>();
                foreach (var item in items)
                {
                    if (!item.IsItem)
                    {
                        var c = await client.GetStringAsync(Constant.url + "shopers/" + item.ShopId.ToString());
                        ss.Add(JsonConvert.DeserializeObject<Shoper>(c));
                        remove.Add(item);
                    }
                }
                foreach (var re in remove)
                {
                    items.Remove(re);
                }
                foreach (var item in items)
                {
                    var b = await client.GetByteArrayAsync(Constant.url + "files/" + item.IntroImage);
                    var ms = new MemoryStream(b);
                    Image img = new Image()
                    {
                        Source = ImageSource.FromStream(() => ms),
                        HeightRequest = 100,
                        WidthRequest = 100,
                        Aspect = Aspect.AspectFill,
                        BackgroundColor = Color.White,
                    };
                    images.Add(img);
                }
                foreach (var item in ss)
                {
                    var b = await client.GetByteArrayAsync(Constant.url + "files/" + item.Avatar);
                    var ms = new MemoryStream(b);
                    Image img = new Image()
                    {
                        Source = ImageSource.FromStream(() => ms),
                        HeightRequest = 100,
                        WidthRequest = 100,
                        Aspect = Aspect.AspectFill,
                        BackgroundColor = Color.White,
                    };
                    bts.Add(img);
                }
            }
            var zz = ss.Zip(bts, (s, i) => new { shop = s, img = i });
            foreach (var item in zz)
            {
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += (object o, EventArgs e) => ChangeToShop(o, e, item.shop);
                Frame fr = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 5,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                fr.GestureRecognizers.Add(tap);
                StackLayout stack = new StackLayout();
                
                item.img.HorizontalOptions = LayoutOptions.Start;
                stack.Children.Add(item.img);
                Label name = new Label()
                {
                    Text = item.shop.Name,
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(30, 0, 0, 0),
                    BackgroundColor = Color.White,
                };
            
                stack.Children.Add(name);
                fr.Content = stack;
                inside.Children.Add(fr);
            }
            var z = items.Zip(images, (it, im) => new { itms = it, imgs = im });
            foreach (var item in z)
            {
                ShopItem si = new ShopItem();
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync(Constant.url + "shopers/" + item.itms.ShopId);
                    Shoper s = JsonConvert.DeserializeObject<Shoper>(content);
                    si = s.Packages[item.itms.NumberofShopInShoper];
                }
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += (object o, EventArgs e) => ChangeDir(o, e, item.itms);
                Frame leftFrame = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 5,
                    HorizontalOptions = LayoutOptions.Center
                };
                leftFrame.GestureRecognizers.Add(tap);
                StackLayout left = new StackLayout();
                
                left.Children.Add(item.imgs);
                Label name = new Label()
                {
                    Text = item.itms.Name,
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(20, 0, 0, 0),
                    BackgroundColor = Color.White,
                };
                left.Children.Add(name);

                Label price = new Label()
                {
                    Text = item.itms.Price.ToString(),
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(20, 0, 0, 0),
                    BackgroundColor = Color.White,
                };
                
                left.Children.Add(price);
                if (DateTime.Now <= si.DiscountUntil)
                {
                    Decimal dp = item.itms.Price * item.itms.DiscountPercent / 100;
                    Label dis = new Label()
                    {
                        Text = dp.ToString(),
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.Start,
                        Margin = new Thickness(20, 0, 0, 0),
                        BackgroundColor = Color.White,
                    };
                    
                    left.Children.Add(dis);
                }
                else
                {
                    si.IsDiscount = false;
                    si.DiscountPercent = 0;

                    using (var client = new HttpClient())
                    {
                        var content = await client.GetStringAsync(Constant.url + "shopers/" + si.ShopID);
                        Shoper s = JsonConvert.DeserializeObject<Shoper>(content);
                        s.Packages[si.N0] = si;
                        await client.PutAsJsonAsync<Shoper>(Constant.url + "shopers/" + si.ShopID, s);
                    }
                }
                leftFrame.Content = left;
                inside.Children.Add(leftFrame);
            }
        }

        private async void ChangeToShop(object o, EventArgs e, Shoper shop)
        {
            await Navigation.PushModalAsync(new Showshop(shop));
        }

        private async void ChangeDir(object o, EventArgs e, Searche s)
        {
            ShopItem shI = new ShopItem();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "shopers/" + s.ShopId.ToString());
                Shoper sh = JsonConvert.DeserializeObject<Shoper>(content);
                shI = sh.Packages[s.NumberofShopInShoper];
            }
            await Navigation.PushModalAsync(new Show(shI,s.NumberofShopInShoper));
        }
    }
}