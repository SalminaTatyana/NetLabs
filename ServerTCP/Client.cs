using ServerTCP.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerTCP
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocet { get; set; }
        PacketReader _packetReader;
        public Client(TcpClient client)
        {
            ClientSocet = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocet.GetStream());
            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();
            Console.WriteLine($"[{DateTime.Now}]:Пользователь {Username} подключен.");

            Task.Run(()=>Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = _packetReader.ReadMessage();
                            Console.WriteLine($"[{DateTime.Now}]:{Username}:{msg}");
                            Program.BroadcastMessage($"{Username}:{msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{DateTime.Now}]:Пользователь {Username} отключен.");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocet.Close();
                    break;
                }
            }
        }
    }
}
