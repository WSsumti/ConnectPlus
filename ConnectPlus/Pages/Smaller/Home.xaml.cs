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
    public partial class Home : ContentPage
    {
        private List<ShopItem> items = new List<ShopItem>();
        private List<Pr> Posts = new List<Pr>();
        int aaa = 1;
        public Home()
        {
            InitializeComponent();
            Ui();
        }

        private async void Ui()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                    User us = JsonConvert.DeserializeObject<User>(content);
                    foreach (var shop in us.FollowingShops)
                    {
                        content = await client.GetStringAsync(Constant.url + "shopers/" + shop.Id.ToString());
                        items = JsonConvert.DeserializeObject<Shoper>(content).Packages;
                        content = await client.GetStringAsync(Constant.url + "pr/" + shop.Id.ToString());
                        var a = JsonConvert.DeserializeObject<List<Pr>>(content);
                        Posts.AddRange(a);
                    }
                }
            }
            catch (Exception)
            {
            }
            StackLayout l = new StackLayout();
            foreach (var item in Posts)
            {
                Shoper s = new Shoper();
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync(Constant.url + "shopers/" + item.ShopId.ToString());
                    s = JsonConvert.DeserializeObject<Shoper>(content);
                }
                Frame fr = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(5, 5, 5, 5),
                };
                StackLayout st = new StackLayout();
                Button name = new Button()
                {
                    Text = s.Name,
                    FontSize = 12,
                    Margin = new Thickness(20, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                name.Clicked += (object sender, EventArgs e) => Name_Clicked(sender, e, s);
                st.Children.Add(name);
                Label con = new Label()
                {
                    Text = item.Content,
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(10, 0, 0, 0),
                };
                st.Children.Add(con);
                Frame f = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 5,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                Image i = new Image()
                {
                    Aspect = Aspect.AspectFit,
                };
                using (var client = new HttpClient())
                {
                    var bb = await client.GetByteArrayAsync(Constant.url + "files/" + item.Picture);
                    var ms = new MemoryStream(bb);
                    i.Source = ImageSource.FromStream(() => ms);
                }
                f.Content = i;
                st.Children.Add(f);
                StackLayout sta = new StackLayout();
                Frame c = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 20,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                StackLayout ee = new StackLayout();
                Editor eee = new Editor()
                {
                    HeightRequest = 100,
                    FontSize = 12,
                };
                ee.Children.Add(eee);
                Button b = new Button()
                {
                    Text = "Enter",
                    FontSize = 12.5,
                    HorizontalOptions = LayoutOptions.End,
                };
                ee.Children.Add(b);
                c.Content = ee;
                st.Children.Add(c);
                Frame anf = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 20,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                StackLayout an = new StackLayout();
                
                Button more = new Button()
                {
                    Text = "Xem thêm bình luận: ",
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.End,
                };
               
                an.Children.Add(more);
                StackLayout ana = new StackLayout();
                Label lll = new Label()
                {
                    Text = item.Comments[item.Comments.Count - 1],
                    FontSize = 12.5,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(10, 10, 0, 0),
                };
                ana.Children.Add(lll);
                b.Clicked += (object sender, EventArgs e) => B_Clicked(sender, e, eee, ana, item);
                more.Clicked += (object sender, EventArgs e) => More_Clicked(sender, e, ana, item, more);
                an.Children.Add(ana);
                anf.Content = an;
                st.Children.Add(anf);
                fr.Content = st;
                l.Children.Add(fr);
            }
            items = items.OrderByDescending(i => i.Access).ToList();
            foreach (var item in items)
            {
                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += (object sender, EventArgs e) => ShowItem(sender, e, item);
                Frame fr = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 5,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                fr.GestureRecognizers.Add(tap);
                StackLayout st = new StackLayout();
                using (var client = new HttpClient())
                {
                    var b = await client.GetByteArrayAsync(Constant.url + "files/" + item.IntroImages[0]);
                    var ms = new MemoryStream(b);

                    Image img = new Image()
                    {
                        Source = ImageSource.FromStream(() => ms),
                        HeightRequest = 200,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Aspect = Aspect.AspectFill,
                    };
                    st.Children.Add(img);
                    Label name = new Label()
                    {
                        Text = item.Name,
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.Start,
                    };
                    st.Children.Add(name);
                    Label price = new Label()
                    {
                        Text = item.Price.ToString(),
                        FontSize = 12,
                        HorizontalOptions = LayoutOptions.Start,
                    };
                    st.Children.Add(price);
                    if (DateTime.Now <= item.DiscountUntil)
                    {
                        Decimal dp = item.Price * (100 - item.DiscountPercent) / 100;
                        Label disprice = new Label()
                        {
                            Text = dp.ToString(),
                            FontSize = 12,
                            HorizontalOptions = LayoutOptions.Start,
                        };
                        price.TextColor = Color.Gray;
                        st.Children.Add(disprice);
                    }
                    else
                    {
                        item.IsDiscount = false;
                        item.DiscountPercent = 0;
                        var content = await client.GetStringAsync(Constant.url + "shopers/" + item.ShopID);
                        Shoper s = JsonConvert.DeserializeObject<Shoper>(content);
                        s.Packages[item.N0] = item;
                        await client.PutAsJsonAsync<Shoper>(Constant.url + "shopers/" + item.ShopID, s);
                    }
                    fr.Content = st;
                    l.Children.Add(fr);
                }
            }
            scroll.Content = l;
            re.IsEnabled = true;
        }

        private void More_Clicked(object sender, EventArgs e, StackLayout s, Pr pr, Button b)
        {
            if (aaa == 1) 
            {
                b.Text = "Thu nhỏ";
                s.Children.Clear();
                foreach (var item in pr.Comments)
                {
                    Label l = new Label()
                    {
                        Text = item,
                        FontSize = 12.5,
                        HorizontalOptions = LayoutOptions.Start,
                        Margin = new Thickness(10, 10, 0, 0),
                    };
                    s.Children.Add(l);
                }
                aaa = 2;
            }
            else
            {
                b.Text = "Xem thêm bình luận:";
                s.Children.Clear();
                Label l = new Label()
                {
                    Text = pr.Comments[pr.Comments.Count-1],
                    FontSize = 12.5,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(10, 10, 0, 0),
                };
                s.Children.Add(l);
                aaa = 1;
            }
        }

        private async void B_Clicked(object sender, EventArgs e, Editor ed, StackLayout s, Pr pr)
        {
            Label l = new Label()
            {
                Text = App.Username + ": " + ed.Text,
                FontSize = 12.5,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(10, 0, 0, 0),
            };
            s.Children.Clear();
            s.Children.Add(l);
            ed.Text = "";
            List<string> ls = pr.Comments;
            ls.Add(l.Text);
            pr.Comments = ls;
            using (var client = new HttpClient())
            {
                await client.PutAsJsonAsync<Pr>(Constant.url + "pr/" + pr.Id.ToString(), pr);
            }
        }

        private async void Name_Clicked(object sender, EventArgs e, Shoper s)
        {
            await Navigation.PushModalAsync(new Showshop(s));
        }

        private async void ShowItem(object sender, EventArgs e, ShopItem item)
        {
            await Navigation.PushModalAsync(new Show(item,item.N0));
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            re.IsEnabled = false;
            try
            {
                scroll.Content = null;
                Posts.Clear();
                items.Clear();
                Ui();
                re.IsRefreshing = false;
            }
            catch (Exception)
            {

            }
        }
    }
}