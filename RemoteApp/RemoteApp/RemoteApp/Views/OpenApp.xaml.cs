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
        ItemsViewModel viewModel;
        public Apps App { get; set; }

        public OpenApp()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
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

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}