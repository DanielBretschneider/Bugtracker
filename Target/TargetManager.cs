using System;
using System.Collections.Generic;

namespace bugracker.Target
{
    class TargetManager
    {
        List<Target> Targets = new List<Target>();

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
