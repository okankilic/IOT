using IOT.Domain.Data.Enums;
using IOT.Domain.Data.Interfaces.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.Domain.Data.Models.Messages
{
    public class SensorInfoMessage : IMessage
    {
        public const MessageType MESSAGE_TYPE = MessageType.Info;
        public const ControllerType CONTROLLER_TYPE = ControllerType.Sensor;

        private const string noKey = "no";
        private const string temperatureKey = "temp";
        private const string humidityKey = "hum";

        public int No
        {
            get
            {
                var arg = Args.Single(q => q.Key == noKey);

                return int.Parse(arg.Value);
            }
        }

        public float Temperature
        {
            get
            {
                var arg = Args.Single(q => q.Key == temperatureKey);

                return float.Parse(arg.Value);
            }
        }

        public float Humidity
        {
            get
            {
                var arg = Args.Single(q => q.Key == humidityKey);

                return float.Parse(arg.Value);
            }
        }

        public SensorInfoMessage(string action, KeyValuePair<string, string>[] args)
        {
            Action = action;
            Args = args;
        }

        public MessageType MessageType => MessageType.Info;

        public ControllerType ControllerType => ControllerType.Sensor;

        public string Action { get; }

        public KeyValuePair<string, string>[] Args { get; }
    }
}
