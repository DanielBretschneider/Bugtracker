using Bugtracker.Attributes;
using Bugtracker.Console;

namespace bugracker.Console.Commands
{
    [Command("send", "snd", "Utility to send captures to target")]
    [Arguments(new[] { "target1", "target2", "..targetx"})]
    class SendCommand : Command
    {
        public override string Execute()
        {
            return "";
        }
    }
}
