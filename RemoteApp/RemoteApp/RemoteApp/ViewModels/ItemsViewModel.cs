using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using RemoteApp.Models;
using RemoteApp.Views;
using System.IO;

namespace RemoteApp.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Item Item { get; private set; }

        public ItemsViewModel()
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
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string filename = Path.Combine(path, "conn.txt");
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        String line = sr.ReadToEnd();
                        ip = line;
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read:");
                }
                Items.Clear();

                tcp server = new tcp();

                if (server.tcpTest(ip))
                {

                    string[] arr = server.tcpProc(ip).Split(',');

                    for (int i = 0, j = 0; i < arr.Length; i += 2)
                    {
                        if (arr[i].ToLower().Contains("host") || arr[i].ToLower().Contains("microsoft") || arr[i].ToLower().Equals("idle")
                            || arr[i].ToLower().Equals("system") || arr[i].ToLower().Equals("registry")) { }
                        else
                        {
                            Item item = new Item
                            {
                                Text = arr[i],
                                Description = arr[i + 1]
                            };
                            Items.Add(item);
                        }
                    }
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