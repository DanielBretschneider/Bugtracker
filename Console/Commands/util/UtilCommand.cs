using Bugtracker.Attributes;
using Bugtracker.Console;
using Bugtracker.Globals_and_Information;
using Bugtracker.GlobalsInformation;
using System.IO;

namespace bugracker.Console.Commands
{
    [Command("util", "utl", "A collection of utitly commands.")]
    [Arguments(new[] { "subcommand" })]
    class UtilCommand : Command
    {
        public override string Execute()
        {
            return base.Execute();
        }
    }

    [Command("init", "init", "Initializes BugtrackerFolder", typeof(UtilCommand))]
    class UtilInitCommand : Command
    {
        public override string Execute()
        {
            string folderName = BugtrackerUtils.CreateBugtrackFolderName();
            DirectoryInfo directory = BugtrackerUtils.CreateBugtrackFolder();

            RunningConfiguration.GetInstance().BugtrackerFolderName = folderName;
            RunningConfiguration.GetInstance().NewestBugtrackerFolder =  directory;
            return "Created Bugtrackerfolder under: " + directory.FullName;
        }
    }
}
