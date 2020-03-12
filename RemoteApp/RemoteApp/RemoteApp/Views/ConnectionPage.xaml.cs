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
    public partial class ConnectionPage : ContentPage
    {
        public ConnectionPage()
        {
            InitializeComponent();
        }
        public async void tcpExec(object sender, System.EventArgs e)
        {
            string s = "";
            IPAddress ip;
            bool ValidateIP = IPAddress.TryParse(IP.Text, out ip);
            if (!ValidateIP)
                alertIP();
            else
            {
                try
                {
                    Int32 port = 8883;
                    TcpClient client = new TcpClient("10.0.0.250", port);

                    Byte[] data = System.Text.Encoding.ASCII.GetBytes("login,Hoff");

                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", "Login,Hoff");
                    data = new Byte[256];

                    String responseData = String.Empty;

                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    s = responseData;

                    stream.Close();
                    client.Close();
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine("ArgumentNullException: {0}", ex);
                }
                catch (SocketException exc)
                {
                    Console.WriteLine("SocketException: {0}", exc);
                }
                if (s.ToLower().Equals("yes"))
                {
                    IP.IsVisible = false;
                    connLbl.IsVisible = false;
                    powerBtn.Text = "Power Off";
                    await DisplayAlert("Alert", "You are now connected!", "OK");
                }
                else
                    alertAsyncConError(s);
            }
        }

        public void tcpPower(object sender, System.EventArgs e)
        {
            if (IP.IsVisible)
            {
                IP.IsVisible = false;
                connLbl.IsVisible = false;
                powerBtn.Text = "Power Off";

                //Do power on stuff here..
                /*
                 * 
                 */

                tcpExec(null, null); //test connection

                //save the connection data entered.
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string filename = Path.Combine(path, "conn.txt");
                using (var streamWriter = new StreamWriter(filename, false))
                {
                    streamWriter.Write(IP.Text);
                }


            }
            else
            {
                IP.IsVisible = true;
                connLbl.IsVisible = true;
                powerBtn.Text = "Power On";
                //Do power on stuff here..
                /*
                 * 
                 */
            }

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