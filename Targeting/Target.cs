using System;

namespace Bugtracker.Targeting
{
    enum TargetType
    {
        folder,
        mail
    }
    class Target
    {
        public bool Default { get; set; }
        public TargetType TargetType { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return "Name: " + this.Name + Environment.NewLine +
                   "Type: " + this.TargetType + Environment.NewLine +
                   "Path: " + this.Path + Environment.NewLine +
                   "Address: " + this.Address + Environment.NewLine +
                   "Default: " + this.Default + Environment.NewLine;
        }
    }
}
