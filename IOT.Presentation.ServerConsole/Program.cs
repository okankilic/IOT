﻿using IOT.Domain.Client;
using IOT.Domain.Data.Impls.Serializers;
using IOT.Domain.Data.Interfaces.Messages;
using IOT.Domain.Data.Models.Messages;
using IOT.Domain.Server;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
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
        static IOTHub hub = new IOTHub();

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
                        if (hub.ClientList.Count > 0)
                        {
                            hub.ClientList[0].Send(cmd);
                        }
                    }
                }
            }

        }

        private static void Server_OnClientConnected(TcpClient tcpClient)
        {
            Console.WriteLine("IOTClient connected");

            var iotClient = new IOTClient(tcpClient);

            iotClient.OnDisconnected += IotClient_OnDisconnected;
            iotClient.OnMessageReceived += IotClient_OnMessageReceived;

            hub.ClientList.Add(iotClient);

            hub.OnIotConnected();

            iotClient.Start();
        }

        private static void IotClient_OnDisconnected(IOTClient client)
        {
            client.OnDisconnected -= IotClient_OnDisconnected;
            client.OnMessageReceived -= IotClient_OnMessageReceived;

            hub.ClientList.Remove(client);

            hub.OnIotDisconnected();

            Console.WriteLine("IOTClient disconnected");
        }

        private static void IotClient_OnMessageReceived(IOTClient sender, string message)
        {
            Console.WriteLine("Message received from client");
            Console.WriteLine(message);

            hub.OnIotMessageReceived(message);
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
            //app.UseCors(CorsOptions.AllowAll);
            //app.MapSignalR();

            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    EnableJSONP = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.

                hubConfiguration.EnableDetailedErrors = true;
                map.RunSignalR(hubConfiguration);
            });
        }
    }

    [HubName("iotHub")]
    public class IOTHub : Hub
    {
        static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<IOTHub>();

        public List<IOTClient> ClientList = new List<IOTClient>();

        public IOTHub()
        {

        }

        public void Send(string str)
        {
            var message = MessageSerializer.Deserialize(str);

            Console.WriteLine(JsonConvert.SerializeObject(message));

            if(ClientList.Count > 0)
            {
                ClientList[0].Send(str);
            }

            Clients.All.addMessage(message);
        }

        public void OnIotConnected()
        {
            hubContext.Clients.All.onConnected();
        }

        public void OnIotDisconnected()
        {
            hubContext.Clients.All.onDisconnected();
        }

        public void OnIotMessageReceived(string str)
        {
            var message = MessageSerializer.Deserialize(str);

            Console.WriteLine(JsonConvert.SerializeObject(message));

            hubContext.Clients.All.onMessageReceived(message);
        }
    }
}
