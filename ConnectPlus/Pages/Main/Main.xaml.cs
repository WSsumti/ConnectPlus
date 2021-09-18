using ConnectPlus.Pages.Smaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : TabbedPage
    {
        public Main()
        {
            InitializeComponent();
            SubHome home = new SubHome()
            {
                Title = "Home",
            };
            SearchTab search = new SearchTab()
            {
                Title = "Search",
            };
            Info info = new Info()
            {
                Title = "Info",
            };
            CartPage cart = new CartPage()
            {
                Title = "Giỏ hàng"
            };
            this.Children.Add(home);
            this.Children.Add(search);
            this.Children.Add(info);
            this.Children.Add(cart);
        }
    }
}