using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker
{
    [Serializable]
    class ParameterNotValidExeption : Exception
    {
        public string Parameter { get; }
        public string ActionType { get; }

        public ParameterNotValidExeption() { }
        public ParameterNotValidExeption(string message) 
            : base(message) { }
        public ParameterNotValidExeption(string message, Exception inner) 
            : base(message, inner) { }

        public ParameterNotValidExeption(string message, string parameter)
            : this(message)
        {
            Parameter = parameter;
        }

        public ParameterNotValidExeption(string message, string parameter, string actionType)
            : this(message)
        {
            Parameter = parameter;
            ActionType = actionType;
        }
    }
}
