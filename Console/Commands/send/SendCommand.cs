﻿using bugracker.Sending;
using bugracker.Targeting;
using Bugtracker.Attributes;
using Bugtracker.Console;
using Bugtracker.GlobalsInformation;
using System.Collections.Generic;

namespace bugracker.Console.Commands
{
    [Command("send", "snd", "Utility to send captures to target")]
    [Arguments(null, new[] { "target1", "target2", "..targetx" })]
    class SendCommand : Command
    {
        public override string Execute()
        {
            TargetManager tm = RunningConfiguration.GetInstance().TargetManager;
            List<Target> targetsToSend = new List<Target>();

            foreach(string target in GivenArguments)
            {
                if (tm.GetTargetByName(target) != null && tm.GetTargetByName(target).Default)
                    targetsToSend.Add(tm.GetTargetByName(target));
                else
                    return "One or more target names are not existing or are writen incorrectly";
            }

            SendHandler sh = new SendHandler(targetsToSend);

            sh.Send();
            return "Sent all current captures to targets.";
        }
    }

    [Command("default", "dft", "Sends to all default targets.", typeof(SendCommand))]
    class SendToDefaultCommand : Command
    {
        public override string Execute()
        {
            TargetManager tm = RunningConfiguration.GetInstance().TargetManager;
            List<Target> targetsToSend = new List<Target>();

            foreach (Target target in tm.Targets)
            {
                if (target.Default)
                    targetsToSend.Add(target);
            }

            SendHandler sh = new SendHandler(targetsToSend);

            (int, int) completionStatus = SendHandler.ReturnCompletionStatus(sh.Send());

            return "Sent current captures to " + completionStatus.Item2 + " of " + completionStatus.Item1 + " default targets.";
        }
    }

}
