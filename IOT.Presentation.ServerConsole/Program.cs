using IOT.Domain.Client;
using IOT.Domain.Data.Impls.Serializers;
using IOT.Domain.Data.Interfaces.Messages;
using IOT.Domain.Data.Models.Messages;
using IOT.Domain.Server;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IOT.Presentation.ServerConsole
{
    class Program
    {
        static List<IOTClient> clientList = new List<IOTClient>();

        static Queue<IOTClientOnMessageReceivedArgs> incomingMessageQueue = new Queue<IOTClientOnMessageReceivedArgs>();
        static Queue<IOTClientOnMessageReceivedArgs> outgoingMessageQueue = new Queue<IOTClientOnMessageReceivedArgs>();

        static void Main(string[] args)
        {
            string url = "http://localhost:1235";

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);

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
                        if (clientList.Count > 0)
                        {
                            clientList[0].Send(cmd);
                            //HandleMessage(clientList[0], cmd);
                        }
                    }
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

            //HandleMessage(sender, message);

            //Console.WriteLine(MessageSerializer.Deserialize(message));
        }

        private static void HandleMessage(IOTClient client, string str)
        {
            var message = MessageSerializer.Deserialize(str);

            switch (message.MessageType)
            {
                case Domain.Data.Enums.MessageType.Command:
                    //outgoingMessageQueue.Enqueue(new IOTClientOnMessageReceivedArgs
                    //{
                    //    Client = client,
                    //    Message = message
                    //});
                    //HandleCommandMessage(message);

                    client.Send(str);
                    break;

                case Domain.Data.Enums.MessageType.Info:
                    incomingMessageQueue.Enqueue(new IOTClientOnMessageReceivedArgs
                    {
                        Client = client,
                        Message = message
                    });
                    //HandleInfoMessage(message);
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

    public class IOTClientOnMessageReceivedArgs
    {
        public IMessage Message { get; set; }

        public IOTClient Client { get; set; }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class MyHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}
