using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading;
using ConnectPlus.Pages.Main;

namespace ConnectPlus.Pages.Decor
{
    public class IntroPage : ContentPage
    {
        Image introImage;
        public IntroPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            introImage = new Image()
            {
                Source = "fff.png",
                WidthRequest = 100,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            StackLayout layout = new StackLayout()
            {
                BackgroundColor = Color.White,
                Children =
                {
                    introImage,
                }
            };
            this.BackgroundColor = Color.White;
            this.Content = layout;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await introImage.ScaleTo(1, 3000);
            //Thread.Sleep(3000);
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
