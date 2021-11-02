using bugracker.Targeting;
using Bugtracker.Globals_and_Information;
using Bugtracker.GlobalsInformation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bugracker.Sending
{
    class SendHandler
    {
        List<Target> targets;
        public SendHandler(List<Target> targets)
        {
            this.targets = targets;
        }
        
        /// <summary>
        /// Returns completion status tuple ex. 1
        /// </summary>
        /// <returns></returns>
        public static (int, int) ReturnCompletionStatus(List<bool> completionStatus)
        {
            int size = completionStatus.Count;
            int complete = 0;

            foreach(bool b in completionStatus)
            {
                if (b == true)
                    complete++;
            }

            return (size, complete);
        }

        public static float ReturnCompletionStatusPercent(List<bool> completionsStatus)
        {
            (int, int) completionsStat = ReturnCompletionStatus(completionsStatus);

            return completionsStat.Item2 / completionsStat.Item1;
        }

        /// <summary>
        /// Sends bugtracker folders either by copy or per mail
        /// </summary>
        /// <returns>The status of sending completion of all folders</returns>
        public List<bool> Send()
        {
            List<bool> completionStatus = new List<bool>();

            foreach (Target t in targets)
            {
                if (t.TargetType == TargetType.folder)
                    completionStatus.Add(SendPerCopy(t));
                else if (t.TargetType == TargetType.mail)
                    completionStatus.Add(SendPerMail(t));
            }

            return completionStatus;
        }
        /// <summary>
        /// Per Default sends all folders created in this bugtracker session per mail
        /// to default target
        /// </summary>
        /// <returns>The status of sending completion of all folders</returns>
        public bool SendPerMail(Target t)
        {
            //TODO Implement in next version.

            return false;
        }

        /// <summary>
        /// Per Default send all folder created in this bugtracker session per copy
        /// to default target
        /// </summary>
        /// <returns>The status of sending completion of all folders</returns>
        public bool SendPerCopy(Target t)
        {
            if((t.Path != null || t.Path != ""))
            {
                if(RunningConfiguration.GetInstance().BugtrackerFolders.Count != 0)
                {
                    foreach (DirectoryInfo di in RunningConfiguration.GetInstance().BugtrackerFolders)
                    {
                        Directory.CreateDirectory(t.Path + "\\" + di.Name);
                        BugtrackerUtils.DirectoryCopy(di.FullName, t.Path + "\\" + di.Name, true);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
