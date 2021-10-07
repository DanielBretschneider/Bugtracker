
using System;
using System.Collections.Generic;
using Bugtracker.Console.Commands;
using Bugtracker.InternalApplication;

namespace Bugtracker.Console
{
    class ApplicationManagerCommand : BaseCommand, ICommand
    {
        private readonly ApplicationManager _applicationManager;
        private readonly ApplicationManagerAction _appManagerAction;

        private readonly List<string> _parameters;

        public ApplicationManagerCommand(ApplicationManager appManager,  List<string> arguments)
        {
            _applicationManager = appManager;
            _appManagerAction = _applicationManager.GetAppManagerActionPerString(arguments[0]);
            arguments.RemoveAt(0);
            _parameters = arguments;
        }

        public string ExecuteAction()
        {
            switch(_appManagerAction)
            {
                case ApplicationManagerAction.list:
                    System.Diagnostics.Debug.WriteLine(_applicationManager.GetApplications().Count);
                    System.Diagnostics.Debug.WriteLine(_applicationManager.ToString());
                    return _applicationManager.ToString();
                    break;
                case ApplicationManagerAction.add:
                    throw new NotImplementedException();
                    break;
                case ApplicationManagerAction.remove:
                    throw new NotImplementedException();
                    break;
                case ApplicationManagerAction.installed:
                    throw new NotImplementedException();
                    break;
                case ApplicationManagerAction.help:
                    return GetAllInfoMessages(typeof(ApplicationManagerAction)); 
                default:
                    throw new CommandActionDoesNotExistEception();
            }
        }

        public override void AddCommandName()
        {
            CommandName = "applications";
        }

        public override void AddHelp()
        {
            HelpMessages.Add(new Commands.HelpMessage((int)ApplicationManagerAction.add,
                "Adds a new Application to the running-configuration.",
                new[] { "appname" }));

            HelpMessages.Add(new Commands.HelpMessage((int)ApplicationManagerAction.installed,
                "Shows all configured Applications that are installed.",
                new[] { "appname" }));

            HelpMessages.Add(new Commands.HelpMessage((int)ApplicationManagerAction.list,
                "Lists all Applications of the running configuration.",
                new[] { "appname" }));

            HelpMessages.Add(new Commands.HelpMessage((int)ApplicationManagerAction.remove,
                "Removes an Application from the running configuration.",
                new[] { "appname" }));

            HelpMessages.Add(new Commands.HelpMessage((int)ApplicationManagerAction.write,
                "Writes all new Application-Changes to the configuraiton file.",
                new[] { "appname" }));

            HelpMessages.Add(new Commands.HelpMessage((int)ApplicationManagerAction.help,
                "Writes this Message",
                new[] { "appname" }));
        }
    }
}
