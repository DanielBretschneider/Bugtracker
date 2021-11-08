using System;
using System.Runtime.Serialization;

namespace Bugtracker.Exceptions
{
    [Serializable]
    internal class ConfigFileParseException : Exception
    {
        public ConfigFileParseException()
        {
        }

        public ConfigFileParseException(string message) : base(message)
        {
        }

        public ConfigFileParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigFileParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string ToString()
        {
            return "XML Configuration is faulty: " + Message;
        }
    }
}