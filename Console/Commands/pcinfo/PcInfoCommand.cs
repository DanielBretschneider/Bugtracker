using Bugtracker.Attributes;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Console.Commands.pcinfo
{
    /// <summary>
    /// Print information of PC 
    /// </summary>
    [Command("pcinfo", "pcinf", "Shows Information on PC(ip, mac, hostname, domain and user)")]
    class PcInfoCommand : Command
    {
        public override string Execute()
        {
            PCInfo pcinfo = new PCInfo();

            // get pc info summary as string
            string pcInfoString = PCInfo.Summary();

            // print to terminal 
            return pcInfoString + Globals.EOL_CHARACTER;
        }
    }
}
