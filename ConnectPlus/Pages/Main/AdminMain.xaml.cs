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
    public partial class AdminMain : TabbedPage
    {
        public AdminMain()
        {
            InitializeComponent();
            ListShopPage lsp = new ListShopPage()
            {
                Title = "Danh sách shop",
            };
            this.Children.Add(lsp);
            ProvenPage pp = new ProvenPage()
            {
                Title = "Chấp nhận mở shop",
            };
            this.Children.Add(pp);

        }
    }
}