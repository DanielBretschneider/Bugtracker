using Bugtracker.Globals_and_Information;
using Bugtracker.Attributes;

namespace Bugtracker.Console.Commands.applications
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
