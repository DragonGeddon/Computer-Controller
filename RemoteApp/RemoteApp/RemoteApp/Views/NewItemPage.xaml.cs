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
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                //Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            string ip = "";
            try
            {
                using (StreamReader sr = new StreamReader("conn.txt"))
                {
                    String line = sr.ReadToEnd();
                    ip = line;
                }
            }
            catch (IOException e2)
            {
                Console.WriteLine("The file could not be read:");
            }
            tcp server = new tcp();
            //MessagingCenter.Send(this, "AddItem", Item);
            bool opener = server.tcpOpen(ip, Item.Text);
            await Navigation.PopModalAsync();
            if(opener)
            {
                await DisplayAlert("Alert", "Could not find the process..", "OK");
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}