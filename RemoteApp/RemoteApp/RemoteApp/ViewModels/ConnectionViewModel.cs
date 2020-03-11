using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RemoteApp.ViewModels
{
    public class ConnectionViewModel : BaseViewModel
    {
        public ConnectionViewModel()
        {
            Title = "Connection";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}