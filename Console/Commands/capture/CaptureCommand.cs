using Bugtracker.Globals_and_Information;
using Bugtracker.Attributes;
using Bugtracker.GlobalsInformation;
using bugracker;
using System.Collections.Generic;
using Bugtracker.Logging;
using Bugtracker.InternalApplication;

namespace Bugtracker.Console.Commands.capture
{
    [Command("capture", "capt", "Captures data from Host PC")]
    [Arguments(null, new[] { "subcommand" })]

    class CaptureCommand : Command
    {
        public override string Execute()
        {
            return GlobalMessages.NOT_IMPLEMENTED;
        }
    }

    [Command("-sequence", "-seq", "Captures screenshot every time the mouse gets clicked.", typeof(CaptureCommand))]
    class CaptureScreenshotSequenceCommand : Command
    {
        public override string Execute()
        {
            BugtrackConsole.Print("Started recording");
            ScreenCaptureHandler sch = new ScreenCaptureHandler();
            sch.GenerateScreenshotSequence();
            BugtrackConsole.Pause();
            return "Stopped recording";
        }
    }

    [Command("-log", "-l", "Captures Log Files only", typeof(CaptureCommand))]
    [Arguments(new[] { "application / all" }, new[] { "application2", "application3..."})]
    class CaptureLogFilesCommand : Command
    {
        public override string Execute()
        {
            BugtrackerUtils.CreateBugtrackFolder();

            List<InternalApplication.Application> targetedApplications = new List<InternalApplication.Application>();
            ApplicationManager am = RunningConfiguration.GetInstance().ApplicationManager;

            foreach (string argument in GivenArguments)
            {   
                if(am.GetApplicationByName(argument) != null)
                    targetedApplications.Add(am.GetApplicationByName(argument));
                else
                    return "One or more given applications do not exist or names do not match!";
            }

            LogfileFetcher lff = new LogfileFetcher(targetedApplications, RunningConfiguration.GetInstance().BugtrackerFolderName);
            lff.FetchAllLogFiles();

            return "Fetched all log files from given applications(s).";
        }
    }

    [Command("-screen", "-s", "Captures screenshots only", typeof(CaptureCommand))]
    class CaptureScreenShotsCommand : Command
    {
        public override string Execute()
        {
            string screenFilePath;
            FetchedScreenshot fetchedScreenshot = new FetchedScreenshot();

            fetchedScreenshot.Directory = BugtrackerUtils.CreateBugtrackFolder();
            fetchedScreenshot.Name = screenFilePath = BugtrackerUtils.GenerateScreenCapture();

            BugtrackConsole.Pause();

            return "Screencapture saved under: " + screenFilePath;
        }
    }

    [Command("-path", "-p", "Shows current screenshot folder path", typeof(CaptureCommand))]
    class GetCapturePathCommand : Command
    {
        public override string Execute()
        {
            return "Screencapture saved under: " + RunningConfiguration.GetInstance().BugtrackerFolderName;
        }
    }
}
