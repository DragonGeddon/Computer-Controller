using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RemoteApp.Models;
using RemoteApp.ViewModels;
using System.IO;

namespace RemoteApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class OpenApp : ContentPage
    {
        AppsViewModel viewModel;
        public Apps App { get; set; }

        public OpenApp()
        {
            InitializeComponent();
            BindingContext = viewModel = new AppsViewModel();
            App = new Apps
            {
                Dir = "Item name",
                //Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "conn.txt");
            string ip = "";
            using (var streamWriter = new StreamReader(filename, false))
            {
                ip = streamWriter.ReadLine();
            }
            tcp server = new tcp();
            //MessagingCenter.Send(this, "AddItem", Item);
            server.tcpOpen(ip, App.Dir);
            await Navigation.PopModalAsync();
        }

        async void OnAppsSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            AppsListView.SelectedItem = null;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}