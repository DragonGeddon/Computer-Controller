using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using RemoteApp.Models;
using RemoteApp.Views;
using System.IO;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;

namespace RemoteApp.ViewModels
{
    public class AppsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Apps App { get; private set; }

        public AppsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                string ip = "";
                try
                {
                    String line = "";
                    string pp = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string fn = Path.Combine(pp, "conn.txt");
                    using (StreamReader sr = new StreamReader(fn))
                    {
                        line = sr.ReadToEnd();
                        ip = line;
                    }
                    line = "";
                    using (StreamReader sr = new StreamReader("shortcuts.txt"))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            Apps app = new Apps();
                            app.Dir = line;
                        }
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read:");
                }

                bool go = false;


                IPAddress IP;
                if (IPAddress.TryParse(ip, out IP))
                {
                    Socket s = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
                    s.ReceiveTimeout = 3000;
                    s.SendTimeout = 3000;
                    try
                    {
                        s.Connect(IP, 8883);
                        go = true;
                        s.Close();
                    }
                    catch (Exception ex)
                    {
                        // something went wrong
                        go = false;
                    }
                }


                Items.Clear();
                if(go) {
                tcp server = new tcp();

                string[] arr = server.tcpProc(ip).Split(',');

                    //for (int i = 0; i < arr.Length; i += 2)
                    //{
                    //    if (arr[i].ToLower().Contains("host") || arr[i].ToLower().Contains("microsoft") || arr[i].ToLower().Equals("idle")
                    //        || arr[i].ToLower().Equals("system") || arr[i].ToLower().Equals("registry")) { }
                    //    else
                    //    {
                    //        Apps App = new Item
                    //        {
                    //            Text = arr[i],
                    //            Description = arr[i + 1]
                    //        };
                    //        Items.Add(item);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}