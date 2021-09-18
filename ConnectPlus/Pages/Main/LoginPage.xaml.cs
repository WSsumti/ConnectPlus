using ConnectPlus.Data;
using ConnectPlus.Pages.Smaller;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public async void LoginButton(object sender, EventArgs e)
        {
            if (AppUser.Text.Trim(' ') != "admin")
            {
                try
                {
                    User user = await UserAuthenticator(AppUser.Text);
                    App.Username = user.Account.Username;
                    App.IsShoper = user.IsShoper;
                    App.Idd = user.Id;
                    if (user.Account.Password == AppPass.Text)
                    {
                        Shoper shoper = await GetShoper(user.Id);
                        Application.Current.MainPage = new NavigationPage(new Main());
                    }
                    else
                    {
                        await DisplayAlert("Thông báo!", "Sai email hoặc mật khẩu.", "Ok");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Thông báo!", "Sai email hoặc mật khẩu.", "Ok");
                } 
            }
            else
            {
                try
                {
                    if (await AdminAuthen(AppPass.Text))
                    {
                        await Navigation.PushModalAsync(new AdminMain());
                    }
                    else
                    {
                        await DisplayAlert("Thông báo!", "Sai email hoặc mật khẩu.", "Ok");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Thông báo!", "Có gì đó không ổn.", "Ok");
                }
            }
        }

        private async Task<bool> AdminAuthen(string pass)
        {
            bool check;
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "admin/" + pass);
                check = JsonConvert.DeserializeObject<bool>(content);
            }
            return check;
        }

        private async Task<User> UserAuthenticator(string user)
        {
            Uri uri = new Uri(Constant.url + "users/" + user);
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(uri)/*.ConfigureAwait(false)*/;
                User tempUser = JsonConvert.DeserializeObject<User>(content);
                return tempUser;
            }
        }

        private async Task<Shoper> GetShoper(Guid id)
        {
            Uri uri = new Uri(Constant.url + "shopers/" + id.ToString());
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(uri)/*.ConfigureAwait(false)*/;
                Shoper tempShoper = JsonConvert.DeserializeObject<Shoper>(content);
                return tempShoper;
            }
        }

        private async void Registration(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegistrationPage());
        }

        private async void ForgotPassword(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPasswordPage());
        }
    }
}