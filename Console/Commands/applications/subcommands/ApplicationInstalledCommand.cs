using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Console.Commands.applications.subcommands
{
    [Command("installed", "Shows all configured Applications that are installed.", typeof(ApplicationManagerCommand))]
    [Arguments(new[] { "appname" })]

    class ApplicationInstalledCommand : Command
    {
        public override string Execute()
        {
            return GlobalMessages.NOT_IMPLEMENTED;
        }
    }
}
