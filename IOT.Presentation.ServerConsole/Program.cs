using IOT.Domain.Client;
using IOT.Domain.Data.Impls.Serializers;
using IOT.Domain.Data.Interfaces.Messages;
using IOT.Domain.Data.Models.Messages;
using IOT.Domain.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.Presentation.ServerConsole
{
    class Program
    {
        static List<IOTClient> clientList = new List<IOTClient>();

        static void Main(string[] args)
        {
            var server = new IOTServer();

            server.OnClientConnected += Server_OnClientConnected;

            server.Start();

            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd.Equals("exit"))
                {
                    server.Stop();
                    break;
                }
                else
                {
                    HandleMessage(cmd);
                }
            }

        }

        private static void Server_OnClientConnected(System.Net.Sockets.TcpClient tcpClient)
        {
            Console.WriteLine("IOTClient connected");

            var iotClient = new IOTClient(tcpClient);

            iotClient.OnDisconnected += IotClient_OnDisconnected;
            iotClient.OnMessageReceived += IotClient_OnMessageReceived;

            clientList.Add(iotClient);

            iotClient.Start();
        }

        private static void IotClient_OnDisconnected(IOTClient client)
        {
            client.OnDisconnected -= IotClient_OnDisconnected;
            client.OnMessageReceived -= IotClient_OnMessageReceived;

            clientList.Remove(client);

            Console.WriteLine("IOTClient disconnected");
        }

        private static void IotClient_OnMessageReceived(IOTClient sender, string message)
        {
            Console.WriteLine("Message received from server");
            Console.WriteLine(message);

            HandleMessage(message);
        }

        private static void HandleMessage(string str)
        {
            var message = MessageSerializer.Deserialize(str);

            switch (message.MessageType)
            {
                case Domain.Data.Enums.MessageType.Command:
                    HandleCommandMessage(message);
                    break;

                case Domain.Data.Enums.MessageType.Info:
                    HandleInfoMessage(message);
                    break;
            }
        }

        private static void HandleCommandMessage(IMessage message)
        {
            if(message is LedOnOffMessage)
            {

            }
        }

        private static void HandleInfoMessage(IMessage message)
        {
            if(message is SensorInfoMessage)
            {

            }
        }
    }
}
