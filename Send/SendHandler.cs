using System;
using Bugtracker.Configuration;
using Bugtracker.Problem_Descriptors;
using Bugtracker.Targeting;
using Bugtracker.Utils;
using System.Collections.Generic;
using System.IO;

namespace Bugtracker.Send
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
                    completionStatus.Add(SendPerCopy(t, problemDescriptor));
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
                bool useCustomBTFolderName = false;
                //Custom Bugtracker Folder Name creation
                if(t.CustomBugtrackerFolderName != null || t.CustomBugtrackerFolderName != "")
                {
                    useCustomBTFolderName = true;

                    if (problemDescriptor?.ProblemCategory != null)
                    {
                        RunningConfiguration.GetInstance().VariableManager.VariableDictionary["ticket"] =
                            (problemDescriptor.ProblemCategory.TicketAbbreviation, false);
                    }

                    t.CustomBugtrackerFolderName = RunningConfiguration.GetInstance().VariableManager.ReplaceKeywords(t.CustomBugtrackerFolderName);
                }

                if (RunningConfiguration.GetInstance().BugtrackerFolders.Count != 0)
                {
                    foreach (DirectoryInfo di in RunningConfiguration.GetInstance().BugtrackerFolders)
                    {
                        string bugtrackerFolderName = di.Name;
                        //Create bugtracker folder at target path
                        if (useCustomBTFolderName)
                            bugtrackerFolderName = t.CustomBugtrackerFolderName;    

                        Directory.CreateDirectory(t.Path + "\\" + bugtrackerFolderName);

                        if(problemDescriptor?.ProblemCategory != null)
                            CreateProblemDescriptionFile(t.Path + "\\" + bugtrackerFolderName + "\\" + problemDescriptor.ProblemCategory.Name + "_Problem_Description", problemDescriptor);

                        //copy content of bugtracker folder to target path
                        BugtrackerUtils.DirectoryCopy(di.FullName, t.Path + "\\" + bugtrackerFolderName, true);

                        //create blackhole folder at target path
                        Directory.CreateDirectory(t.Path + "\\" + bugtrackerFolderName + "\\blackhole");

                        //copy content of blackhole folder to target path

                        BugtrackerUtils.DirectoryCopy(Globals_and_Information.Globals.LOCAL_BLACKHOLE_FODLER_PATH, t.Path + "\\" + bugtrackerFolderName + "\\blackhole", true);
                    }

                    

                    return true;
                }
            }

            return false;
        }

        public void CreateProblemDescriptionFile(string path, ProblemDescriptor problemDescriptor)
        {
            using (StreamWriter sw = File.CreateText(path + ".txt"))
            {
                System.Diagnostics.Debug.WriteLine("Created Problem Description file....");
                sw.WriteLine("Problem Kategorie");
                sw.WriteLine(problemDescriptor.ProblemCategory.Name);
                sw.WriteLine("---------------------------------------------" + Environment.NewLine);
                sw.WriteLine("Problem Beschreibung:");
                sw.WriteLine(problemDescriptor.ProblemDescription);
            }
        }
    }
}
