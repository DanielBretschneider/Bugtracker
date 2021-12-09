using Bugtracker.Globals_and_Information;
using Bugtracker.Targeting;
using System.Collections.Generic;
using System.IO;
using Bugtracker.Configuration;
using Bugtracker.Problem_Descriptors;
using Bugtracker.Utils;

namespace Bugtracker.Sending
{
    /// <summary>
    /// Providing methods to send Bugtracker folders of local session to given targets.
    /// </summary>
    public class SendHandler
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

            foreach (bool b in completionStatus)
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
        public List<bool> Send(ProblemDescriptor problemDescriptor = null)
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
        public bool SendPerMail(Target t, ProblemDescriptor problemDescriptor = null)
        {
            //TODO Implement in next version.

            return false;
        }

        /// <summary>
        /// Per Default send all folder created in this bugtracker session per copy
        /// to default target
        /// </summary>
        /// <returns>The status of sending completion of all folders</returns>
        public bool SendPerCopy(Target t, ProblemDescriptor problemDescriptor = null)
        {
            if ((t.Path != null || t.Path != ""))
            {
                if (RunningConfiguration.GetInstance().BugtrackerFolders.Count != 0)
                {
                    foreach (DirectoryInfo di in RunningConfiguration.GetInstance().BugtrackerFolders)
                    {
                        Directory.CreateDirectory(t.Path + "\\" + di.Name);

                        if(problemDescriptor != null)
                            CreateProblemDescriptionFile(t.Path + "\\" + di.Name + "\\" + problemDescriptor.ProblemCategory + "_Problem_Description", problemDescriptor);

                        BugtrackerUtils.DirectoryCopy(di.FullName, t.Path + "\\" + di.Name, true);
                    }

                    return true;
                }
            }

            return false;
        }

        public void CreateProblemDescriptionFile(string path, ProblemDescriptor problemDescriptor)
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(problemDescriptor.ProblemDescription);
            }
        }
    }
}
