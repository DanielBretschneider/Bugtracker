using Bugtracker.Console.Commands;
using System;
using System.Collections.Generic;

namespace Bugtracker.Console
{
    /// <summary>
    /// Holds methods and values every command should inherit
    /// ex: GetHelp, returns string how to use given command
    /// </summary>
    class BaseCommand
    {
        public List<HelpMessage> HelpMessages;
        public string CommandName { get; set; }
        public string Parameters { get; set; }

        public BaseCommand()
        {
            HelpMessages = new List<HelpMessage>();
            AddHelp();
            AddCommandName();
        }

        public virtual void AddHelp()
        {
            throw new System.NotImplementedException("Please override in subclass! And Add Help Messages");
        }

        public virtual void AddCommandName()
        {
            throw new System.NotImplementedException("Please override in subclass! And Add CommandName");
        }

        public string GetInfoMessage(HelpMessage hm, Type actionType)
        {
            return CommandName + " " +
                Enum.GetName(actionType,
                hm.ActionType) + " \t" +
                hm.Message + System.Environment.NewLine;
        }

        public string GetAllInfoMessages(Type actionType)
        {
            //TODO: write more elegant version with less recursion
            string allHelp = "";

            foreach (HelpMessage hm in HelpMessages)
            {
                allHelp += GetInfoMessage(hm, actionType);
            }

            return allHelp;
        }

        public string GetHelp(HelpMessage hm, Type actionType)
        {
            string baseCommand = CommandName;
            string[] requiredParams = hm.RequiredParams;
            string[] optionalParams = hm.OptionalParams;


            string helpMessage = System.Environment.NewLine;
            helpMessage += "Use like: " + baseCommand + " " + Enum.GetName(actionType ,hm.ActionType);

            for(int i = 0; i < requiredParams.Length; i++)
            {
                helpMessage += " <" + requiredParams[i] + ">";
            }

            helpMessage += ", optional [";

            for(int i = 0; i < optionalParams.Length; i++)
            {
                helpMessage += " <" + optionalParams[i] + ">";
            }

            helpMessage += "]";

            return helpMessage;
        }

        public HelpMessage GetHelpMessageForAction(int action)
        {
            foreach(HelpMessage hm in HelpMessages)
            {
                if (hm.ActionType == action)
                    return hm;
            }

            //TODO: Maybe, write its own exception....
            throw new System.Exception("Wron Action Code");
        }
    }
}
