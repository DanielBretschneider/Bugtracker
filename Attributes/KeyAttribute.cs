using System;

namespace Bugtracker.Attributes
{
    internal class KeyAttribute : Attribute
    {
        public string Name { get; set; }


        public KeyAttribute(string keyName)
        {
            Name = keyName;
        }
    }
}