using Bugtracker.Globals_and_Information;
using Bugtracker.Attributes;
using Bugtracker.GlobalsInformation;
using bugracker;
using System.Collections.Generic;
using Bugtracker.Logging;
using Bugtracker.InternalApplication;
using System.Diagnostics;
using System;

namespace Bugtracker.Console.Commands.capture
{
    [Command("capture", "capt", "Captures data from Host PC")]
    [Arguments(null, new[] { "subcommand" })]

    class CaptureCommand : Command
    {
        public override string RootExecution()
        {
            BugtrackerUtils.CreateBugtrackFolder();
            return "Bugtracker folder created!";
        }
        public override string Execute()
        {
            return new CaptureFullCommand().Execute();
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
            List<InternalApplication.Application> targetedApplications = new List<InternalApplication.Application>();
            ApplicationManager am = RunningConfiguration.GetInstance().ApplicationManager;

            foreach (string argument in GivenArguments)
            {   
                if(am.GetApplicationByName(argument) != null)
                    targetedApplications.Add(am.GetApplicationByName(argument));
                else
                    return "One or more given applications do not exist or names do not match!";
            }

            LogfileFetcher lff = new LogfileFetcher(targetedApplications);
            lff.FetchAllLogFiles();

            return "Fetched all log files from given applications(s).";
        }
    }

    [Command("all", "-a", "Captures log files from all installed applications", typeof(CaptureLogFilesCommand))]
    class CaptureAllLogFilesCommand : Command
    {
        public override string Execute()
        {
            List<InternalApplication.Application> targetedApplications = new List<InternalApplication.Application>();
            ApplicationManager am = RunningConfiguration.GetInstance().ApplicationManager;

            foreach (Application a in am.GetApplications())
            {
                if (a.IsInstalled)
                    targetedApplications.Add(a);
            }

            System.Diagnostics.Debug.WriteLine("targeted Applications all: " + targetedApplications.Count);

            LogfileFetcher lff = new LogfileFetcher(targetedApplications);
            lff.FetchAllLogFiles();

            return "Fetched all log files from all installed applications(s).";
        }
    }

    [Command("-screen", "-s", "Captures screenshots only", typeof(CaptureCommand))]
    class CaptureScreenShotsCommand : Command
    {
        public override string Execute()
        {
            string screenFilePath;

            screenFilePath = BugtrackerUtils.GenerateScreenCapture();

            BugtrackConsole.Pause();

            return "Screencapture saved under: " + screenFilePath;
        }
    }

    [Command("-full", "-s", "Captures all log files and makes screenshots", typeof(CaptureCommand))]
    class CaptureFullCommand : Command
    {
        public override string Execute()
        {
            CaptureScreenShotsCommand captureScreenShotsCommand = new CaptureScreenShotsCommand();
            CaptureAllLogFilesCommand captureAllLogFilesCommand = new CaptureAllLogFilesCommand();

            string result = "";

            result += captureScreenShotsCommand.Execute() + Environment.NewLine;
            result += captureAllLogFilesCommand.Execute() + Environment.NewLine;

            return result;
        }
    }


    [Command("-path", "-p", "Shows current screenshot folder path", typeof(CaptureCommand))]
    class ShowCapturePathCommand : Command
    {
        public override string Execute()
        {
            return "Screencapture saved under: " + RunningConfiguration.GetInstance().NewestBugtrackerFolder;
        }
    }

    [Command("open", "o", "Opens current screenshot folder path", typeof(ShowCapturePathCommand))]
    class OpenCapturePathCommand : Command
    {
        public override string Execute()
        {
            if (RunningConfiguration.GetInstance().NewestBugtrackerFolder != null)
            {
                Process.Start(RunningConfiguration.GetInstance().NewestBugtrackerFolder.FullName);
                return "Opening Path";
            }
            else
                return "No path set yet!";
        }
    }
}
