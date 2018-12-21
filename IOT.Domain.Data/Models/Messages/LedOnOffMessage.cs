using IOT.Domain.Data.Enums;
using IOT.Domain.Data.Interfaces.Messages;
using System.Collections.Generic;

namespace IOT.Domain.Data.Models.Messages
{
    public class LedOnOffMessage : IMessage
    {
        public const MessageType MESSAGE_TYPE = MessageType.Command;
        public const ControllerType CONTROLLER_TYPE = ControllerType.Led;

        public LedOnOffMessage(string action, KeyValuePair<string, string>[] args)
        {
            Action = action;
            Args = args;
        }

        public MessageType MessageType => MESSAGE_TYPE;

        public ControllerType ControllerType => ControllerType;

        public string Action { get; }

        public KeyValuePair<string, string>[] Args { get; }
    }
}
