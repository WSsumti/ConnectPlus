using ConnectPlus.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConnectPlus.Pages.Smaller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatRoom : ContentPage
    {
        ChatViewModel vm;
        public ChatViewModel VM
        {
            get => vm ?? (vm = (ChatViewModel)BindingContext);
        }
        public ChatRoom()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.ConnectCommand.Execute(null);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.DisconnectCommand.Execute(null);
        }
    }
}