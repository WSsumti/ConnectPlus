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
    public partial class PrCreater : ContentPage
    {
        FileResult image;
        private int n = -1;
        public PrCreater()
        {
            InitializeComponent();
            item.Clicked += Item_Clicked;
            save.Clicked += (object sender, EventArgs e) => Save_Clicked(sender, e, cont.Text, check.IsChecked);
        }

        private async void Item_Clicked(object sender, EventArgs e)
        {
            ItemforPr i = new ItemforPr();
            await Navigation.PushModalAsync(i);
            n = i.no;
        }

        private async void Save_Clicked(object sender, EventArgs e, string contents, bool isPR)
        {
            if (((contents == null) && (contents == "") && (image == null)) || ((isPR)&&(n==-1)))
            {
                await DisplayAlert("Thông báo:", "Thiếu dữ liệu", "Ok");
            }
            else 
            {
                Pr pr = new Pr()
                {
                    IsPr = isPR,
                    Content = contents,
                    ShopId = App.Idd,
                    N0 = n,
                    Comments = new List<string>(),
                };
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(await image.OpenReadAsync()), "file", image.FileName);
                pr.Picture = "storage--" + App.Username + "--Picture--Shop/" + Path.GetFileNameWithoutExtension(image.FullPath) + ".png";
                using (HttpClient client = new HttpClient())
                {
                    await client.PostAsJsonAsync<Pr>(Constant.url + "pr", pr);
                    await client.PostAsync(Constant.url + "files/storage--" + App.Username + "--Picture--Shop/" + Path.GetFileNameWithoutExtension(image.FullPath) + "/image", content);
                }
                await DisplayAlert("Thông báo:", "Tạo thành công", "Ok");
                await Navigation.PopModalAsync();
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                });
                if (file == null)
                    return;
                image = file;
            }
            catch (Exception)
            {
            }
        }
    }
}