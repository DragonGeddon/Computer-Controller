using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RemoteApp.ViewModels
{
    class tcp
    {

        public bool tcpTest(string Ip)
        {
            string s = "";
            IPAddress ip;
            bool ValidateIP = IPAddress.TryParse(Ip, out ip);
            try
            {
                Int32 port = 8883;
                TcpClient client = new TcpClient(Ip, port);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes("login,Hoff");

                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", "Login,Hoff");
                data = new Byte[256];

                String responseData = String.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                if (responseData.ToLower().Equals("yes"))
                {
                    return true;
                }
                else
                    return false;

                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("ArgumentNullException: {0}", ex);
                return false;
            }
            catch (SocketException exc)
            {
                Console.WriteLine("SocketException: {0}", exc);
                return false;
            }
        }

        public string tcpProc(string ip)
        {
            try
            {
                Int32 port = 8883;
                TcpClient client = new TcpClient(ip, port);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes("proc,Hoff");

                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                data = new Byte[4096];

                String responseData = String.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                return responseData;

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
            return "";
        }

        public bool tcpKill(string ip, int id)
        {
            try
            {
                Int32 port = 8883;
                TcpClient client = new TcpClient(ip, port);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes("kill,"+id);

                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

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
            return true;
        }

        public bool tcpOpen(string ip, string id)
        {
            try
            {
                Int32 port = 8883;
                TcpClient client = new TcpClient(ip, port);
                NetworkStream stream = client.GetStream();
                Byte[] data = System.Text.Encoding.ASCII.GetBytes("open,"+id);

                data = new Byte[4096];

                String responseData = String.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                if (responseData.ToLower().Equals("false"))
                    return false;
                stream.Write(data, 0, data.Length);

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
            return true;
        }
    }
}
