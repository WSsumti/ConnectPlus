using ConnectPlus.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Smaller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayShop : ContentPage
    {
        MultipartFormDataContent AvaContent;
        MultipartFormDataContent CoverContent;
        int aaa = 1;
        public DisplayShop(bool hasAShop)
        {
            InitializeComponent();
            StackLayout layout = new StackLayout();
            if (hasAShop)
            {
                UI1(layout);
            }
            else
            {
                UI2(layout);
            }
            this.Content = layout;
        }
        private async void UI1(StackLayout llayout)
        {
            StackLayout lll = new StackLayout();
            StackLayout layout = new StackLayout();
            ScrollView s = new ScrollView();
            StackLayout l = new StackLayout()
            {
                HeightRequest = 250,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Red,
            };
            string foldername = "storage--" + App.Username + "--Picture--Shop/";
            using (var client = new HttpClient())
            {
                var b = await client.GetByteArrayAsync(Constant.url + "files/" + foldername + "cover.png");
                var ms = new MemoryStream(b);

                Image img = new Image()
                {
                    Source = ImageSource.FromStream(() => ms),
                    HeightRequest = 250,
                };
                l.Children.Add(img);
            }

            lll.Children.Add(l);
            llayout.Children.Add(lll);
            StackLayout ll = new StackLayout()
            {
                HeightRequest = 130,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            using (var client = new HttpClient())
            {
                var b = await client.GetByteArrayAsync(Constant.url + "files/" + foldername + "avatar.png");
                var ms = new MemoryStream(b);

                Image img = new Image()
                {
                    Source = ImageSource.FromStream(() => ms),
                    HeightRequest = 130,
                    WidthRequest = 130,
                    HorizontalOptions = LayoutOptions.Center,
                    Aspect= Aspect.AspectFill,
                };
                ll.Children.Add(img);
            }
            layout.Children.Add(ll);

            Button action = new Button()
            {
                Text = "Thao tác kinh doanh: ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            action.Clicked += Action_Clicked;
            layout.Children.Add(action);

            Button chat = new Button()
            {
                Text = "Xem tin nhắn cho shop: ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            chat.Clicked += Chat_Clicked;
            layout.Children.Add(chat);

            Label shoplist = new Label()
            {
                Text = "Danh sách mặt hàng:",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(20, 0, 0, 0),
            };
            layout.Children.Add(shoplist);

            List<ConcvertPackages> convert = new List<ConcvertPackages>();
            StackLayout itemlayout = new StackLayout()
            {
                BackgroundColor = Color.Red,
                HeightRequest = 200,
            };
            CarouselView itemview = new CarouselView();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "shopers/" + App.Idd);
                Shoper shoper = JsonConvert.DeserializeObject<Shoper>(content);
                foreach (var file in shoper.Packages)
                {
                    var b = await client.GetByteArrayAsync(Constant.url + "files/" + file.IntroImages[0]);
                    var ms = new MemoryStream(b);

                    Image img = new Image()
                    {
                        Source = ImageSource.FromStream(() => ms),
                        HeightRequest = 130,
                        WidthRequest = 130,
                        HorizontalOptions = LayoutOptions.Center,
                    };
                    ConcvertPackages temp = new ConcvertPackages()
                    {
                        Access = file.Access,
                        Description = file.Description,
                        DiscountPercent = file.DiscountPercent,
                        DiscountUntil = file.DiscountUntil,
                        Id = file.Id,
                        IntroImages = img,
                        IsDiscount = file.IsDiscount,
                        Name = file.Name,
                        NBuyers = file.NBuyers,
                        Price = file.Price.ToString(),
                        Rating = file.Rating,
                        N0 = file.N0,
                    };
                    convert.Add(temp);
                }
                
            }
            itemview.PeekAreaInsets = new Thickness(100);
            itemview.Loop = false;
            itemview.ItemsSource = convert;
            itemview.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
            {
                ItemSpacing = 10,
            };
            itemview.ItemTemplate = new DataTemplate(() =>
            {
                Image image = new Image()
                {
                    HeightRequest = 120,
                    WidthRequest = 150,
                    Aspect = Aspect.AspectFill,
                };
                image.SetBinding(Image.SourceProperty, "IntroImages.Source");
                Label name = new Label()
                {
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Start,
                };
                name.SetBinding(Label.TextProperty, "Name");
                Label price = new Label()   
                {
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Start,
                };
                price.SetBinding(Label.TextProperty, "Price");
                Label discount = new Label()
                {
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Start,
                };
                discount.SetBinding(Label.TextProperty, "DiscountPercent");
                StackLayout l = new StackLayout()
                {
                    Children = { image, name, price }
                };
                Frame frame = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 5,
                };
                frame.Content = l;
                StackLayout root = new StackLayout()
                {
                    Children = { frame }
                };
                Label nouse = new Label();
                nouse.SetBinding(Label.TextProperty, "N0");
                var tapp = new TapGestureRecognizer();
                tapp.Tapped += (object sender, EventArgs e) => Tapp_Tapped(sender, e, nouse.Text);
                return root;
            });
            itemlayout.Children.Add(itemview);
            layout.Children.Add(itemlayout);
            List<Pr> prs = new List<Pr>();
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "pr/" + App.Idd);
                prs = JsonConvert.DeserializeObject<List<Pr>>(content);
            }
            StackLayout la = new StackLayout();
            foreach (var item in prs)
            {
                Shoper ss = new Shoper();
                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync(Constant.url + "shopers/" + item.ShopId.ToString());
                    ss = JsonConvert.DeserializeObject<Shoper>(content);
                }
                Frame fr = new Frame()
                {
                    BorderColor = Color.Black,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(5, 5, 5, 5),
                };
                StackLayout st = new StackLayout();
                Button namee = new Button()
                {
                    Text = ss.Name,
                    FontSize = 12,
                    Margin = new Thickness(20, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.Start,
                };
                namee.Clicked += (object sender, EventArgs e) => Name_Clicked(sender, e, ss);
                st.Children.Add(namee);
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
                Label llll = new Label()
                {
                    Text = item.Comments[item.Comments.Count - 1],
                    FontSize = 12.5,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(10, 10, 0, 0),
                };
                ana.Children.Add(llll);
                b.Clicked += (object sender, EventArgs e) => B_Clicked(sender, e, eee, ana, item);
                more.Clicked += (object sender, EventArgs e) => More_Clicked(sender, e, ana, item, more);
                an.Children.Add(ana);
                anf.Content = an;
                st.Children.Add(anf);
                fr.Content = st;
                la.Children.Add(fr);
            }
            layout.Children.Add(la);
            s.Content = layout;
            RefreshView rr = new RefreshView() { Content = s, };
            llayout.Children.Add(rr);
        }

        private async void Chat_Clicked(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "shopers/" + App.Idd);
                Shoper s = JsonConvert.DeserializeObject<Shoper>(content);
                ChatSetting.Username = s.Name;
            }
            await Navigation.PushModalAsync(new ChatLobby());
        }

        private void Tapp_Tapped(object sender, EventArgs e, string n0)
        {
            int N0 = Convert.ToInt32(n0);
        }

        private async void Action_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Hành động", "Cancel", null, "Thêm mặt hàng", "Thêm bài viết");
            switch(action)
            {
                case "Thêm mặt hàng":
                    await Navigation.PushModalAsync(new ShopCreater());
                    break;
                case "Thêm bài viết":
                    await Navigation.PushModalAsync(new PrCreater());
                    break;
            };
        }

        private void UI2(StackLayout llayout)
        {
            StackLayout layout = new StackLayout();
            Label title = new Label()
            {
                Text = "Tạo shop",
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center,
            };
            layout.Children.Add(title);
            StackLayout namelayout = new StackLayout();
            Label name = new Label()
            {
                Text = "Tên shop: ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(18, 0, 0, 0),
            };
            namelayout.Children.Add(name);
            Entry nameentry = new Entry()
            {
                FontSize = 12,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(18, 0, 0, 0),
            };
            namelayout.Children.Add(nameentry);
            Frame nameframe = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                Content = namelayout,
            };
            layout.Children.Add(nameframe);
            StackLayout quoteLayout = new StackLayout();
            Label quote = new Label()
            {
                Text = "Giới thiệu ngắn gọn về shop",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(18, 0, 0, 0),
            };
            quoteLayout.Children.Add(quote);
            Entry quoteEntry = new Entry()
            {
                FontSize = 12,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(18, 0, 0, 0),
            };
            quoteLayout.Children.Add(quoteEntry);
            Frame quoteFrame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                Content = quoteLayout,
            };
            layout.Children.Add(quoteFrame);

            StackLayout avaLayout = new StackLayout();
            Label ava = new Label()
            {
                Text = "Chọn 1 tấm hình để đặt làm ảnh đại diện: ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(18, 0, 0, 0),
            };
            avaLayout.Children.Add(ava);
            Button chooseAva = new Button()
            {
                Text = "Chọn ảnh: ",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(15, 0, 0, 0),
            };

            avaLayout.Children.Add(chooseAva);
            Entry avaname = new Entry()
            {
                FontSize = 12,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(18, 0, 20, 0),
                InputTransparent = true,
            };
            chooseAva.Clicked += (object o, EventArgs e) => ChooseAva_Clicked(o, e, avaname);
            avaLayout.Children.Add(avaname);
            Frame avaFrame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                Content = avaLayout,
            };
            layout.Children.Add(avaFrame);

            StackLayout coverLayout = new StackLayout();
            Label cover = new Label()
            {
                Text = "Chọn 1 tấm hình để đặt làm ảnh nền đại diện: ",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(18, 0, 0, 0),
            };
            coverLayout.Children.Add(cover);
            Button choosecover = new Button()
            {
                Text = "Chọn ảnh: ",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(15, 0, 0, 0),
            };
            coverLayout.Children.Add(choosecover);
            Entry covername = new Entry()
            {
                FontSize = 12,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(18, 0, 20, 0),
                InputTransparent = true,
            };
            choosecover.Clicked += (object o, EventArgs e) => Choosecover_Clicked(o, e, covername);
            coverLayout.Children.Add(covername);
            Frame coverFrame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                Content = coverLayout,
            };
            layout.Children.Add(coverFrame);

            StackLayout phoneLayout = new StackLayout();
            Label phone = new Label()
            {
                Text = "Số điện thoại liên lạc",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(18, 0, 0, 0),
            };
            phoneLayout.Children.Add(phone);
            Entry phoneEntry = new Entry()
            {
                FontSize = 12,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(18, 0, 0, 0),
            };
            phoneLayout.Children.Add(phoneEntry);
            Frame phoneFrame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                Content = phoneLayout,
            };
            layout.Children.Add(phoneFrame);

            StackLayout emailLayout = new StackLayout();
            Label email = new Label()
            {
                Text = "Email liên lạc",
                FontSize = 13,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(18, 0, 0, 0),
            };
            emailLayout.Children.Add(email);
            Entry emailEntry = new Entry()
            {
                FontSize = 12,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(18, 0, 0, 0),
            };
            emailLayout.Children.Add(emailEntry);
            Frame emailFrame = new Frame()
            {
                BorderColor = Color.Black,
                CornerRadius = 5,
                Content = emailLayout,
            };
            layout.Children.Add(emailFrame);

            Button save = new Button()
            {
                Text = "Kiểm tra",
                FontSize = 12,
                HorizontalOptions = LayoutOptions.Center,
            };
            save.Clicked += (object o, EventArgs e) => Save_Clicked(o, e, nameentry.Text, phoneEntry.Text, emailEntry.Text, quoteEntry.Text );
            layout.Children.Add(save);
            
            ScrollView s = new ScrollView();
            s.Content = layout;
            llayout.Children.Add(s);
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
        private async Task<bool> CheckName(string name)
        {
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "shopers/checkshop/" + name);
                bool r = JsonConvert.DeserializeObject<bool>(content);
                return r;
            }
        }

        private async void Save_Clicked(object sender, EventArgs e, string zName, string zPhone, string zEmail, string zquote)
        {
            bool check = await CheckName(zName);
            if (!check)
            {
                Request request = new Request()
                {
                    Name = zName,
                    Email = zEmail,
                    Phone = zPhone,
                };

                Searche searche = new Searche()
                {
                    IsItem = false,
                    Name = zName,
                    ShopId = App.Idd,
                    NumberofShopInShoper = 0,
                    IntroImage = null,
                    ItemId = null,
                    NBuyers = 0,
                    Price = 0,
                };
                OnlyString keyword = new OnlyString()
                {
                    Keyword = zName,
                };
                Shoper shoper = new Shoper()
                {
                    Id = App.Idd,
                    Name = zName,
                    Information = new Data.Info()
                    {
                        Email = zEmail,
                        nFollowers = 0,
                        nLikers = 0,
                        Income = 0,
                        Phone = zPhone,
                        Followers = new List<User>(),
                        Likers = new List<User>(),
                        Web = null,
                    },
                    Quote = zquote,
                    Avatar = "storage--" + App.Username + "--Picture--Shop/avatar.png",
                    CoverPicture = "storage--" + App.Username + "--Picture--Shop/cover.png",
                    Packages = new List<ShopItem>(),
                };

                if (AvaContent == null)
                {
                    shoper.Avatar = "";
                }
                if (CoverContent == null)
                {
                    shoper.CoverPicture = "";
                }

                Report report = new Report()
                {
                    ShopId = App.Idd,
                    ShopName = zName,
                    TotalIncome = 0,
                };

                using (var client = new HttpClient())
                {
                    await client.PostAsync(Constant.url + "files/storage--" + App.Username + "--Picture--Shop/avatar/image", AvaContent);
                    await client.PostAsync(Constant.url + "files/storage--" + App.Username + "--Picture--Shop/cover/image", CoverContent);
                    await client.PostAsJsonAsync<Request>(Constant.url + "pendings/pendingrequest", request);
                    await client.PostAsJsonAsync<Shoper>(Constant.url + "shopers", shoper);
                    await client.PostAsJsonAsync<Searche>(Constant.url + "searches", searche);
                    var c = await client.GetStringAsync(Constant.url + "users/" + App.Username);
                    var u = JsonConvert.DeserializeObject<User>(c);
                    //u.IsShoper = true;
                    await client.PutAsJsonAsync<User>(Constant.url + "users/" + App.Username, u);
                    await client.PostAsJsonAsync<OnlyString>(Constant.url + "searches/keyword", keyword);
                    await client.PostAsJsonAsync<Report>(Constant.url + "reports", report);
                }
                await DisplayAlert("Thông báp: ", "Đã gửi yêu cầu đến ban quản lý", "Ok");
                await Navigation.PopModalAsync(); 
            }
            else
            {
                await DisplayAlert("Thông báp: ", "Đã có tên shop này", "Ok");
            }
        }

        private async void Choosecover_Clicked(object sender, EventArgs e, Entry entry)
        {
            var file = await MediaPicker.PickPhotoAsync();
            if (file == null)
                return;
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(await file.OpenReadAsync()), "file", file.FileName);
            CoverContent = content;
            entry.Text = file.FileName;
        }

        private async void ChooseAva_Clicked(object sender, EventArgs e, Entry entry)
        {
            var file = await MediaPicker.PickPhotoAsync();
            if (file == null)
                return;
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(await file.OpenReadAsync()), "file", file.FileName);
            AvaContent = content;
            entry.Text = file.FileName;
        }
        private /*async*/ void Name_Clicked(object sender, EventArgs e, Shoper s)
        {
            //await Navigation.PushModalAsync(new Showshop(s));
        }
    }
}