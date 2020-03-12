using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Sockets;
using RemoteApp.ViewModels;

namespace RemoteApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CMD : ContentPage
    {
        public CMD()
        {
            InitializeComponent();
        }

        public async void tcpPower(object sender, System.EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filename = Path.Combine(path, "conn.txt");
            string ip = "";
            using (var streamWriter = new StreamReader(filename, false))
            {
                ip = streamWriter.ReadLine();
            }
            tcp server = new tcp();
            await DisplayAlert("Response: ", server.tcpCMD(ip, command.Text), "OK");
        }

        public async System.Threading.Tasks.Task alertAsyncConError(string s)
        {
            await DisplayAlert("Alert", "The Connection could not be made!", "OK");
        }

        public async System.Threading.Tasks.Task alertIP()
        {
            await DisplayAlert("Alert", "Enter a valid IP!", "OK");
        }
    }
}