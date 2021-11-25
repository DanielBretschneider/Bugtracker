using System;
using System.Collections.Generic;
using Bugtracker.Configuration;

namespace Bugtracker.Targeting
{
    /// <summary>
    /// 
    /// </summary>
    public class TargetManager
    {
        public List<Target> Targets { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TargetManager()
        {
            Targets = new List<Target>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Target> GetDefaultTargets()
        {
            List<Target> defaultTargets = new List<Target>();

            foreach (Target target in Targets)
            {
                if (target.Default)
                    defaultTargets.Add(target);
            }

            return defaultTargets;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Target GetTargetByName(string name)
        {
            foreach (Target t in Targets)
            {
                if (t.Name == name)
                    return t;
            }
            return null;
        }

        public override string ToString()
        {
            string returnString = "";

            foreach (Target t in Targets)
            {
                returnString += t.ToString() + Environment.NewLine;
            }

            return returnString;
        }
    }
}
