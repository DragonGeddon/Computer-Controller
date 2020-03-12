using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RemoteApp.ViewModels
{
    class tcp
    {
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

                stream.Close();
                client.Close();
                return responseData;

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("ArgumentNullException: {0}", ex);
            }
            catch (SocketException exc)
            {
                Console.WriteLine("SocketException: {0}", exc);
            }
            return null;
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

                Byte[] data = System.Text.Encoding.ASCII.GetBytes("open,"+id);

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
    }
}
