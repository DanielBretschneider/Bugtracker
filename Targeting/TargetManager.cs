using Bugtracker.GlobalsInformation;
using System;
using System.Collections.Generic;

namespace bugracker.Targeting
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
            foreach(Target t in Targets)
            {
                if(t.Name == name)
                    return t;
            }
            return null;
        }

        public override string ToString()
        {
            string returnString = "";

            foreach(Target t in Targets)
            {
                returnString += t.ToString() + Environment.NewLine;
            }

            return returnString;
        }
    }
}
