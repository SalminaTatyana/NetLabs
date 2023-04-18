using ServerTCP.Net.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerTCP
{
    class Program
    {
        static List<Client> _users;
        static TcpListener _listener;
        static async Task Main(string[] args)
        {
            _users = new List<Client>();
            Connection connection=await JsonDescripter();
            _listener = new TcpListener(IPAddress.Any, connection.port);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                BroadcastConnection();
            }
        }
        static async Task<Connection> JsonDescripter()
        {
            string ip = "127.0.0.1";
            //string ipConsole;
            //string portConsele;
            int port = 55556;
            //Console.WriteLine("Введите Ip");
            //try
            //{
            //    ipConsole= Console.ReadLine();
            //    IPAddress.Parse(ipConsole);
            //    ip = ipConsole;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //Console.WriteLine("Введите Порт");
            //try
            //{
            //    portConsele = Console.ReadLine();
            //    port=Int32.Parse(portConsele);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            try
            {
                
                using (FileStream fs = new FileStream("appsetting.json", FileMode.OpenOrCreate))
                {
                    
                    Connection connection = await JsonSerializer.DeserializeAsync<Connection>(fs);
                    if (!String.IsNullOrEmpty(connection.ip))
                    {
                        ip = connection.ip;
                        port = connection.port;

                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Connection(ip, port);
        }

        static void BroadcastConnection()
        {
            foreach (var user in _users)
            {
                foreach (var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocet.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocet.Client.Send(msgPacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString()==uid).FirstOrDefault();
            _users.Remove(disconnectedUser);
            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocet.Client.Send(broadcastPacket.GetPacketBytes());
            }
            BroadcastMessage($"{disconnectedUser.Username} отключен.");
        }
    }
}
