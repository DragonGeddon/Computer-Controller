using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Sockets;

namespace RemoteApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class PowerShell : ContentPage
    {
        public PowerShell()
        {
            InitializeComponent();
        }
        //public async void tcpExec(object sender, System.EventArgs e)
        //{

        //}

        public void tcpPower(object sender, System.EventArgs e)
        {
            

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