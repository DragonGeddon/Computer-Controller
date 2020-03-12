using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
public class Program
{
    public static void Main(string[] args)
    {
        run();
    }
    public static void run()
    {
        int port = 8883;
        String ip = "0.0.0.0";
        Socket ServerListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
        ServerListener.Bind(ep);
        ServerListener.Listen(100);
        Console.WriteLine("Server is online...");
        Socket ClientSocket = default(Socket);
        Program p = new Program();
        while (true)
        {

            ClientSocket = ServerListener.Accept();
            byte[] msg = new byte[1024];
            int size = ClientSocket.Receive(msg, 0, msg.Length, SocketFlags.None);
            string raw = System.Text.Encoding.ASCII.GetString(msg);
            string[] data = raw.Split(',');
            try
            {
                Console.WriteLine(data[1] + " connected under the IP:" + ClientSocket.RemoteEndPoint + " attempting to call the " + data[0] + " method.");
            } catch (Exception)
            {
            }
                if (data[0].ToLower().Equals("login"))
            {
                //Handler
                ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes("yes"), 0, System.Text.Encoding.ASCII.GetBytes("yes").Length, SocketFlags.None);
                Console.WriteLine("sent login");
            }
            else if (data[0].ToLower().Equals("proc"))
            {
                string list = "";
                Process[] processlist = Process.GetProcesses();

                foreach (Process theprocess in processlist)
                {
                    list += "" + theprocess.ProcessName + "," + theprocess.Id + ",";
                }
                ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(list), 0, System.Text.Encoding.ASCII.GetBytes(list).Length, SocketFlags.None);
                Console.WriteLine("sent processes");
            }
            else if (data[0].ToLower().Equals("kill"))
            {
                Process localById = Process.GetProcessById(Int32.Parse(data[1]));
                System.Diagnostics.Process[] procs = null;
                localById.Kill();
            }
            else if (data[0].ToLower().Equals("open"))
            {
                try
                {
                    var prc = Process.Start(data[1]);
                    prc.WaitForInputIdle();
                }
                catch (Exception e)
                {
                    ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes("false"), 0, System.Text.Encoding.ASCII.GetBytes("false").Length, SocketFlags.None);
                    Console.WriteLine("couldn't open process");
                }
            }

            else
            {
                Console.WriteLine("Device Connected!");
            }
        }
    }
}
