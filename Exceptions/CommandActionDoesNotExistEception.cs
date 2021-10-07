using System;
using System.Runtime.Serialization;

namespace Bugtracker
{
    [Serializable]
    internal class CommandActionDoesNotExistEception : Exception
    {
        public string Command { get; }
        public string ActionType { get; }
        public CommandActionDoesNotExistEception()
        {
        }

        public CommandActionDoesNotExistEception(string message) : base(message)
        {
        }

        public CommandActionDoesNotExistEception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CommandActionDoesNotExistEception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CommandActionDoesNotExistEception(string message, string command)
        : this(message)
        {
            Command = command;
        }

        public CommandActionDoesNotExistEception(string message, string command, string actionType)
        : this(message)
        {
            Command = command;
            ActionType = actionType;
        }

        public override string ToString()
        {
            return "Given command-action does not exist for command: " + Command + ", write: " + Command + " help for a list of all command actions.";
        }
    }
}