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
    public partial class ShopCreater : ContentPage
    {
        IEnumerable<FileResult> ImageFiles;
        IEnumerable<FileResult> VideoFiles;
        public ShopCreater()
        {
            InitializeComponent();
            desImg.Clicked += (object o, EventArgs e) => DesImg_Clicked(o, e, desImgLayout);
            desVideo.Clicked += (object o, EventArgs e) => DesVideo_Clicked(o, e, desVideoLayout);
            datepicker.Date = DateTime.Now;
            save.Clicked += (object sender, EventArgs e) => Save_Clicked(sender,e,name.Text,price.Text,isdiscount.IsChecked,discount.Text, datepicker.Date ,DesString.Text, Keyword.Text, quantity.Text);
        }

        private async void Save_Clicked(object sender, EventArgs e, string name, string price, bool isdiscount, string discountpercent, DateTime discountEXP, string description, string keyword, string quan)
        {

            if ((name == null) || (price == null) || (ImageFiles.Count() == 0) || (description == null) || (keyword == null))
            {
                await DisplayAlert("Cảnh báo: ", "Nhập thiếu thông tin - Vui lòng xem lại", "Ok");
            }
            else
            {
                OnlyString k = new OnlyString()
                {
                    Keyword = keyword,
                };
                ShopItem item = new ShopItem()
                {
                    Name = name,
                    Price = Convert.ToInt32(price),
                    IsDiscount = isdiscount,
                    Description = description,
                    Access = 0,
                    Rating = 0,
                    NBuyers = 0,
                    QuantityInShop = Convert.ToInt32(quan),
                };
                if (!isdiscount)
                {
                    item.DiscountPercent = 0;
                }
                else
                {
                    if (discountpercent == null)
                    {
                        await DisplayAlert("Thông báo", "Nhập thiếu dữ liệu. Vui lòng xem lại", "Ok");
                        return;
                    }
                    item.DiscountPercent = Convert.ToInt32(discountpercent);
                    item.DiscountUntil = discountEXP;
                }

                List<string> imgfile = new List<string>();
                List<string> vidfile = new List<string>();

                if (ImageFiles != null)
                {
                    foreach (var file in ImageFiles)
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(await file.OpenReadAsync()), "file", file.FileName);

                        using (HttpClient client = new HttpClient())
                        {
                            await client.PostAsync(Constant.url + "files/storage--" + App.Username + "--Picture--Shop/" + Path.GetFileNameWithoutExtension(file.FullPath)+ "/image", content);
                        }
                        
                        imgfile.Add("storage--" + App.Username + "--Picture--Shop/" + Path.GetFileNameWithoutExtension(file.FullPath) + ".png");
                    }
                }

                if (VideoFiles != null)
                {
                    foreach (var file in VideoFiles)
                    {
                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(await file.OpenReadAsync()), "file", file.FileName);

                        using (HttpClient client = new HttpClient())
                        {
                            await client.PostAsync(Constant.url + "files/storage--" + App.Username + "--Video--Shop/" + Path.GetFileNameWithoutExtension(file.FullPath) + "/video", content);
                        }
                        Path.GetFileNameWithoutExtension(file.FullPath);
                        vidfile.Add("storage--" + App.Username + "--Video--Shop/" + Path.GetFileNameWithoutExtension(file.FullPath)+ ".mp4");
                    }
                }
                item.IntroImages = imgfile;
                item.IntroVideos = vidfile;
                Searche search = new Searche()
                {
                    Name = name,
                    NBuyers = 0,
                    Price = Convert.ToInt32(price),
                    IntroImage = imgfile[0],
                    IsItem = true,
                };

                using (var client = new HttpClient())
                {
                    var content = await client.GetStringAsync(Constant.url + "shopers/" + App.Idd);
                    Shoper shoper = JsonConvert.DeserializeObject<Shoper>(content);
                    item.N0 = shoper.Packages.Count();
                    item.ShopID = shoper.Id;
                    shoper.Packages.Add(item);
                    search.NumberofShopInShoper = shoper.Packages.Count() - 1;
                    search.ShopId = shoper.Id;
                    await client.PutAsJsonAsync<Shoper>(Constant.url + "shopers/" + App.Idd, shoper);
                    await client.PostAsJsonAsync<Searche>(Constant.url + "searches", search);
                    await client.PostAsJsonAsync<OnlyString>(Constant.url + "searches/keyword", k);
                } 
            }
            await DisplayAlert("Thông báo", "Đăng sản phẩm thành công", "Ok");
            await Navigation.PopModalAsync();
        }

        private async void DesVideo_Clicked(object sender, EventArgs e, StackLayout layout)
        {
            var files = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Videos,
            });
            if (files.Count() == 0)
                return;
            VideoFiles = files;
            foreach (var file in files)
            {
                Grid g = new Grid()
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition
                        {
                            Width = new GridLength(0.5,GridUnitType.Star),
                        },
                    }
                };
                Label label = new Label()
                {
                    Text = file.FileName,
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(20, 0, 0, 0),
                };
                Button button = new Button()
                {
                    Text = "X",
                    FontSize = 14,
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(0, 0, 20, 0),
                };
                Grid.SetColumn(label, 0);
                Grid.SetColumn(button, 1);
                button.Clicked += (object sender, EventArgs e) => VidButton_Clicked(sender,e, g, desVideoLayout, file);
                g.Children.Add(label);
                g.Children.Add(button);
                layout.Children.Add(g);
            }
        }

        private void ImgButton_Clicked(object sender, EventArgs e, Grid g, StackLayout layout, FileResult file)
        {
            layout.Children.Remove(g);
            List<FileResult> img = new List<FileResult>(ImageFiles);
            img.Remove(file);
            ImageFiles = img;
        }
        private void VidButton_Clicked(object sender, EventArgs e, Grid g, StackLayout layout, FileResult file)
        {
            layout.Children.Remove(g);
            List<FileResult> vid = new List<FileResult>(VideoFiles);
            vid.Remove(file);
            VideoFiles = vid;
        }

        private async void DesImg_Clicked(object sender, EventArgs e, StackLayout layout)
        {
            var files = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
            });
            if (files.Count() == 0)
                return;
            ImageFiles = files;
            foreach (var file in files)
            {
                Grid g = new Grid()
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition
                        {
                            Width = new GridLength(0.5,GridUnitType.Star),
                        },
                    }
                };
                Label label = new Label()
                {
                    Text = file.FileName,
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(20, 0, 0, 0),
                };
                Button button = new Button()
                {
                    Text = "X",
                    FontSize = 14,
                    HorizontalOptions = LayoutOptions.End,
                    Margin = new Thickness(0, 0, 20, 0),
                };
                Grid.SetColumn(label, 0);
                Grid.SetColumn(button, 1);
                button.Clicked += (object sender, EventArgs e) => ImgButton_Clicked(sender, e, g, desImgLayout, file);
                g.Children.Add(label);
                g.Children.Add(button);
                layout.Children.Add(g);
            }
        }
    }
}