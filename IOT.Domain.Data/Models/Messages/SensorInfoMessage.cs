using IOT.Domain.Data.Enums;
using IOT.Domain.Data.Interfaces.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOT.Domain.Data.Models.Messages
{
    public class SensorInfoMessage : IMessage
    {
        public const MessageType MESSAGE_TYPE = MessageType.Info;
        public const ControllerType CONTROLLER_TYPE = ControllerType.Sensor;
        
        private const string temperatureKey = "temp";
        private const string humidityKey = "hum";
        private const string waterLevelKey = "water_level";
        private const string soilHumidityKey = "soil_humidity";
        private const string totalMiliLitresKey = "total_mili_litres";
        private const string wateringStatusKey = "watering_status";
        private const string fertiLevelKey = "ferti_level";
        private const string totalFertiKey = "total_ferti";
        private const string fertiStatusKey = "ferti_status";

        public DateTime MessageTime { get; set; }

        public float Temperature
        {
            get
            {
                var arg = Args.Single(q => q.Key == temperatureKey);

                return float.Parse(arg.Value, new CultureInfo(ConfigurationManager.AppSettings["lang"]));
            }
        }

        public float Humidity
        {
            get
            {
                var arg = Args.Single(q => q.Key == humidityKey);

                return float.Parse(arg.Value, new CultureInfo(ConfigurationManager.AppSettings["lang"]));
            }
        }

        public float WaterLevel
        {
            get
            {
                var arg = Args.Single(q => q.Key == waterLevelKey);

                return float.Parse(arg.Value);
            }
        }

        public int SoilHumidity
        {
            get
            {
                var arg = Args.Single(q => q.Key == soilHumidityKey);

                return int.Parse(arg.Value);
            }
        }

        public float TotalMiliLitres
        {
            get
            {
                var arg = Args.Single(q => q.Key == totalMiliLitresKey);

                return float.Parse(arg.Value);
            }
        }

        public float FertiLevel
        {
            get
            {
                var arg = Args.Single(q => q.Key == fertiLevelKey);

                return float.Parse(arg.Value);
            }
        }

        public float TotalFerti
        {
            get
            {
                var arg = Args.Single(q => q.Key == totalFertiKey);

                return float.Parse(arg.Value);
            }
        }

        public int FertiStatus
        {
            get
            {
                var arg = Args.Single(q => q.Key == fertiStatusKey);

                return int.Parse(arg.Value);
            }
        }

        public int RelayStatus
        {
            get
            {
                var arg = Args.Single(q => q.Key == wateringStatusKey);

                return int.Parse(arg.Value);
            }
        }

        public SensorInfoMessage(string action, KeyValuePair<string, string>[] args)
        {
            MessageTime = DateTime.Now;
            Action = action;
            Args = args;
        }

        public MessageType MessageType => MessageType.Info;

        public ControllerType ControllerType => ControllerType.Sensor;

        public string Action { get; }

        public KeyValuePair<string, string>[] Args { get; }
    }
}
