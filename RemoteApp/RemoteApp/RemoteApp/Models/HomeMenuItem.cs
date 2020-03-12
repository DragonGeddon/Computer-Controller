using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteApp.Models
{
    public enum MenuItemType
    {
        Browse,
        Connection,
        PowerShell,
        CMD
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
