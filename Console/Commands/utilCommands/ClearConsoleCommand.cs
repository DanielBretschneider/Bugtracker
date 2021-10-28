using Bugtracker.Attributes;

namespace Bugtracker.Console.Commands.utilCommands
{
    /// <summary>
    /// Clear Terminal screen
    /// </summary>
    [Command("clear", "clr", "Clear console screen")]
    class ClearConsoleCommand : Command
    {
        public override string Execute()
        {
            // Works like "clear" in terminal
            // oder "cls" in cmd
            System.Console.Clear();
            return "";
        }
    }
}
