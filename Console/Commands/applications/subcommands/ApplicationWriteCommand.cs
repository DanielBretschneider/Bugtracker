using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Console.Commands.applications.subcommands
{
    [Command("write", "Writes all new Application-Changes to the configuraiton file.", typeof(ApplicationManagerCommand))]
    class ApplicationWriteCommand : Command
    {
        public override string Execute()
        {
            return GlobalMessages.NOT_IMPLEMENTED;
        }
    }
}
