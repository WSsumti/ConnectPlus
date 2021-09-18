using ConnectPlus.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Smaller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchTab : ContentPage
    {
        public ObservableCollection<OnlyString> SearchList { get; set; }
        private List<OnlyString> filter = new List<OnlyString>();
        public ICommand Search { get; }
        public SearchTab()
        {
            InitializeComponent();
            
            SearchList = new ObservableCollection<OnlyString>();
            SearchBar bar = new SearchBar();
            bar.TextChanged += SearchBar_TextChanged;
            bar.SearchButtonPressed += (object o, EventArgs e) => Bar_SearchButtonPressed(o,e,bar.Text);
            layout.Children.Add(bar);
            CollectionView col = new CollectionView();
            col.SetBinding(CollectionView.ItemsSourceProperty, "SearchList");
            col.ItemTemplate = new DataTemplate(() =>
            {
                StackLayout st = new StackLayout();
                Button search = new Button()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    FontSize = 12, 
                    BackgroundColor = Color.White,
                };
                search.SetBinding(Button.TextProperty, "Keyword");
                search.Clicked += (object o, EventArgs e) => Searchss(o, e, search.Text);
                st.Children.Add(search);
                return st;
            });
            layout.Children.Add(col);
            BindingContext = this;
        }

        private async void Bar_SearchButtonPressed(object sender, EventArgs e, string keyword)
        {
            await Navigation.PushModalAsync(new SearchResult(keyword));
        }

        private async void Searchss(object o, EventArgs e, string keyword)
        {
            await Navigation.PushModalAsync(new SearchResult(keyword));
        }

        
        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchList.Clear();
            var search = e.NewTextValue;
            if (string.IsNullOrWhiteSpace(search))
            {
                search = string.Empty;
                SearchList.Clear();
                return;
            }
            search = search.ToLowerInvariant();
            using (HttpClient client = new HttpClient())
            {
                var content = await client.GetStringAsync(Constant.url + "searches/keyword/" + search);
                filter = JsonConvert.DeserializeObject<List<OnlyString>>(content);
            }
            if (filter != null)
            {
                foreach (var item in filter)
                {
                    SearchList.Add(item);
                } 
            }
        }

    }

}