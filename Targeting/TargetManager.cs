using System;
using System.Collections.Generic;
using Bugtracker.Configuration;

namespace Bugtracker.Targeting
{
    class TargetManager
    {
        public List<Target> Targets { get; set; }

        public TargetManager()
        {
            Targets = ConfigHandler.GetSpecifiedTargets();
        }


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
