using System;
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
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        Item selectedItem;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;

        }

        public async void killTask(object sender, EventArgs e)
        {
            tcp server = new tcp();
            string temp = procID.Text;
            string ip = "";
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string filename = Path.Combine(path, "conn.txt");
                using (StreamReader sr = new StreamReader(filename))
                {
                    String line = sr.ReadToEnd();
                    ip = line;
                }
            }
            catch (IOException e2)
            {
                Console.WriteLine("The file could not be read:");
            }
            server.tcpKill(ip, Int32.Parse(procID.Text));

        }
    }
}