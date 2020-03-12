using RemoteApp.Models;
using RemoteApp.Services;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RemoteApp.ViewModels
{
    public class PowerShellViewModel : BaseViewModel
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public PowerShellViewModel()
        {
            Title = "PowerShell";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}