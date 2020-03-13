using System;
using System.Text;
using System.Threading;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System.Security;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using Shell32;
using System.Net.NetworkInformation;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Globalization;

namespace RPIComputerController
{
    class Program
    {
        private static List<FileInfo> files = new List<FileInfo>();
        private static List<string> filePaths = new List<string>();
        static void Main(string[] args)
        {
            /*            foreach (var file in new DirectoryInfo(@"C:\Users\Spencer Crawford\Desktop").GetFiles("*.url"))
                        {
                            files.Add(file);
                            filePaths.Add(file.FullName);
                        }*//*

            foreach (var file in new DirectoryInfo(@"C:\Users\Spencer Crawford\Desktop").GetFiles("*.lnk"))
            {
                files.Add(file);
                filePaths.Add(file.FullName);
            }

            using (StreamWriter sw = new StreamWriter("C:/Users/Spencer Crawford/Documents/GitHub/ComputerController/shortcuts.txt"))
            {
                foreach (string Path in filePaths)
                {
                    sw.WriteLine(Path);
                }
            }

            //startApp(filePaths[0] + ".exe");*/

            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = "http://stackoverflow.com";
            proc.Start();

        }

        public class WOLClass : UdpClient
        {
            public WOLClass() : base()
            { }

            public void SetClientToBrodcastMode()
            {
                if (this.Active)
                    this.Client.SetSocketOption(SocketOptionLevel.Socket,
                                              SocketOptionName.Broadcast, 0);
            }
        }

        private static void getMackAddress(string MAC_ADDRESS)
        {
            String firstMacAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
            WakeFunction(firstMacAddress);
        }

    private static void WakeFunction(string MAC_ADDRESS)
        {
            WOLClass client = new WOLClass();
            client.Connect(new
               IPAddress(0xffffffff),
               0x2fff);
            client.SetClientToBrodcastMode();
            //set sending bites
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // more than enough :-)
                                             //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2),
                        NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
        }

        private static void startApp(string filename)
        {

            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = filename;

            psi.Arguments = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "boot.ini");

            Process.Start(psi);
        }

        private static void startWebPage(string url)
        {
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = url;
            proc.Start();
        }

        private static void shutdown(bool doRestart)
        {
            if (doRestart)
            {
                Process.Start("shutdown","/r /t 0");
            }
            else
            {
                Process.Start("shutdown", "/s /t 0");
            }
        }

        private static string GetShortcutTarget(string file)
        {
            try
            {
                if (System.IO.Path.GetExtension(file).ToLower() != ".lnk")
                {
                    throw new Exception("Supplied file must be a .LNK file");
                }

                FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                using (System.IO.BinaryReader fileReader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                    uint flags = fileReader.ReadUInt32();        // Read flags
                    if ((flags & 1) == 1)
                    {                      // Bit 1 set means we have to
                                           // skip the shell item ID list
                        fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                        uint offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                        fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                    }

                    long fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                                                                 // structure begins
                    uint totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                    fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                    uint fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                                                               // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                    fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin); // Seek to beginning of
                                                                                        // base pathname (target)
                    long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2; // read
                                                                                                        // the base pathname. I don't need the 2 terminating nulls.
                    char[] linkTarget = fileReader.ReadChars((int)pathLength); // should be unicode safe
                    var link = new string(linkTarget);

                    int begin = link.IndexOf("\0\0");
                    if (begin > -1)
                    {
                        int end = link.IndexOf("\\\\", begin + 2) + 2;
                        end = link.IndexOf('\0', end) + 1;

                        string firstPart = link.Substring(0, begin);
                        string secondPart = link.Substring(end);

                        return firstPart + secondPart;
                    }
                    else
                    {
                        return link;
                    }
                }
            }
            catch
            {
                return "";
            }
        }
    }
}