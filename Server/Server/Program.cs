using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Management.Automation.Runspaces;
using System.Management.Automation;

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
            else if (data[0].ToLower().Equals("powershell"))
            {
                // create Powershell runspace
                Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();

                RunspaceInvoke runSpaceInvoker = new RunspaceInvoke(runspace);
                runSpaceInvoker.Invoke("Set-ExecutionPolicy Unrestricted");

                // create a pipeline and feed it the script text
                Pipeline pipeline = runspace.CreatePipeline();
                //Command command = new Command(SCRIPT_PATH);

                //command.Parameters.Add(null, outputFilename);
                //pipeline.Commands.Add(command);

                pipeline.Invoke();
                runspace.Close();
            }
            else if (data[0].ToLower().Equals("cmd"))
            {
                //string result= "";
                Process pro = new Process();
                pro.StartInfo.FileName = "cmd.exe";
                pro.StartInfo.Arguments = data[1] + "\n";
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.RedirectStandardError = true;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.Start();
                pro.BeginOutputReadLine();
                pro.OutputDataReceived += (_, e) => ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(e.Data.ToString()), 0, System.Text.Encoding.ASCII.GetBytes(e.Data.ToString()).Length, SocketFlags.None);
                //ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes("endend"), 0, System.Text.Encoding.ASCII.GetBytes("endend").Length, SocketFlags.None);
            }
            else if (data[0].ToLower().Equals("shutdown"))
            {
                Process.Start("shutdown", "/s /t 0");
            }
            else
            {
                Console.WriteLine("Device Connected!");
            }
        }
    }
}
