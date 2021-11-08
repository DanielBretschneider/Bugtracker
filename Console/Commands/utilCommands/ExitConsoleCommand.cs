using Bugtracker.Attributes;

namespace Bugtracker.Console.Commands.utilCommands
{
    /// <summary>
    /// Sets Console Running bool to false -> console loop closes
    /// </summary>
    [Command("exit", "exit", "Exits the command line")]
    class ExitConsoleCommand : Command
    {
        public override string Execute()
        {
            BugtrackConsole.ConsoleRunning = false;
            return "";
        }
    }
}
