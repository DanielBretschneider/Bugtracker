using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker.Console.Commands
{
    class HelpMessage
    {
        public int ActionType { get; set; }
        public string Message { get; set; }
        public string[] RequiredParams { get; set; }
        public string[] OptionalParams { get; set;  }

        string[] optional = new[] { "" };

        public HelpMessage(int actionType, string helpMessage, string[] requiredParams)
        {
            ActionType = actionType;
            Message = helpMessage;
            RequiredParams = requiredParams;
        }

        public HelpMessage(int actionType, string helpMessage, string[] requiredParams, string[] optionalParams)
        {
            ActionType = actionType;
            Message = helpMessage;
            RequiredParams = requiredParams;
            OptionalParams = optionalParams;
        }
    }
}
