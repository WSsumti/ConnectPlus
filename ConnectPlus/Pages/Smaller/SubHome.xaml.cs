using ConnectPlus.Data;
using ConnectPlus.Data.DTO;
using ConnectPlus.ViewModel;
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
    public partial class SubHome : ContentPage
    {
        SubHomeModel view;
        private List<Pr> Posts = new List<Pr>();
        int aaa = 1;
        public SubHome()
        {
            InitializeComponent();
            BindingContext = view = new SubHomeModel();
            GetItem();
        }
        private async void GetItem()
        {
            List<ItemDTO> itss = new List<ItemDTO>();
            try
            {
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                    User us = JsonConvert.DeserializeObject<User>(content);
                    foreach (var shop in us.FollowingShops)
                    {
                        var contentt = await client.GetStringAsync(Constant.url + "shopers/" + shop.Id.ToString());
                        var its = JsonConvert.DeserializeObject<Shoper>(contentt).Packages;
                        var cont = await client.GetStringAsync(Constant.url + "pr/" + shop.Id.ToString());
                        var a = JsonConvert.DeserializeObject<List<Pr>>(cont);
                        Posts.AddRange(a);
                        foreach (var item in its)
                        {
                            ItemDTO dto = new ItemDTO()
                            {
                                ShopID = shop.Id,
                                Name = item.Name,
                                Price = item.Price * (100-item.DiscountPercent) / 100,
                                N0 = item.N0,
                            };
                            var b = await client.GetByteArrayAsync(Constant.url + "files/" + item.IntroImages[0]);
                            var ms = new MemoryStream(b);
                            Image img = new Image()
                            {
                                Source = ImageSource.FromStream(() => ms),
                            };
                            dto.Image = img;
                            itss.Add(dto);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            view.Items = itss;
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
                StackLayout namelayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                };
                using (var client = new HttpClient())
                {
                    var bb = await client.GetByteArrayAsync(Constant.url + "files/" + s.Avatar);
                    var ms = new MemoryStream(bb);
                    Image ii = new Image()
                    {
                        Source = ImageSource.FromStream(() => ms),
                        Aspect = Aspect.AspectFill,
                    };
                    Frame frr = new Frame()
                    {
                        HeightRequest = 35,
                        WidthRequest = 35,
                        CornerRadius = 30,
                        Padding = 0,
                        Margin = 5,
                        Content = ii,
                    };
                    namelayout.Children.Add(frr);
                }
                Label name = new Label()
                {
                    Text = s.Name,
                    FontSize = 20,
                    Margin = new Thickness(20, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                    FontAttributes = FontAttributes.Bold,
                };
                //name.Clicked += (object sender, EventArgs e) => Name_Clicked(sender, e, s);
                namelayout.Children.Add(name);
               
                st.Children.Add(namelayout);
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
                    BackgroundColor = Color.FromHex("#F7F5F0"),
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
                    BackgroundColor = Color.FromHex("#F7F5F0"),
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
                layout.Children.Add(fr);
            }
        }
        private async void Name_Clicked(object sender, EventArgs e, Shoper s)
        {
            await Navigation.PushModalAsync(new Showshop(s));
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
                    Text = pr.Comments[pr.Comments.Count - 1],
                    FontSize = 12.5,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(10, 10, 0, 0),
                };
                s.Children.Add(l);
                aaa = 1;
            }
        }
    }
}