using ConnectPlus.Data;
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
    public partial class Info : ContentPage
    {
        public Info()
        {
            InitializeComponent();
            UI();
        }

        private async void UI()
        {
            user.Text = App.Username;
            if (App.IsShoper)
                number.Text = "Số người theo dõi: " + App.F;
            try
            {
                using (var client = new HttpClient())
                {
                    var i = await client.GetByteArrayAsync(Constant.url + "files/" + App.Username + "--Picture--Personal" + "/" + "avatar.png");
                    var ms = new MemoryStream(i);
                    img.Source = ImageSource.FromStream(() => ms);

                }
            }
            catch (Exception)
            {
                img.Source = "dummyava.png";
            }
        }
        private async void NavigateToShop(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DisplayShop(App.IsShoper));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DeliveryItem());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            ChatSetting.Username = App.Username;
            await Navigation.PushModalAsync(new ChatLobby());
        }
    }
}