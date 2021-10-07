using System;
using System.Runtime.Serialization;

namespace Bugtracker.InternalApplication
{
    [Serializable]
    internal class ApplicationNotFoundException : Exception
    {
        public string ApplicationParameter { get;  }
        public ApplicationNotFoundException()
        {
        }

        public ApplicationNotFoundException(string parameter) : base(parameter)
        {
            ApplicationParameter = parameter;
        }

        public ApplicationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ApplicationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string ToString()
        {
            return "Given application does not exist, write: \"applications list\" for a list of all applications.";
        }
    }
}