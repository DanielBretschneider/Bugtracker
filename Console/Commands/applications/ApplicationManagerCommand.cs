using Bugtracker.Attributes;

namespace Bugtracker.Console
{
    [Command("applications", "apps", "Utility to manage applications")]
    [Arguments(new[] { "subcommand"}, new[] { "optionalParams"})]
    class ApplicationManagerCommand : Command
    {
        public override string Execute()
        {
            System.Diagnostics.Debug.WriteLine("manager command execute");
            return GetAllHelpMessages();
        }
    }
}
