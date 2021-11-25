using System;

namespace Bugtracker.Targeting
{
    public enum TargetType
    {
        folder,
        mail
    }

    public class Target
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Default { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TargetType TargetType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>
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
