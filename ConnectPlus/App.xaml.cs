using ConnectPlus.Data;
using ConnectPlus.Pages.Decor;
using ConnectPlus.Pages.Smaller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus
{
    public partial class App : Application
    {
        public static string Username { get; set; }
        public static string Group { get; set; }
        public static int L { get; set; }
        public static int F { get; set; }
        public static bool IsShoper = false;
        public static Guid Idd { get; set; }
        public static ObservableCollection<Order> Orders = new ObservableCollection<Order>();
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new IntroPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
