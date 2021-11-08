using System.IO;

namespace Bugtracker.Capture
{
    class FetchedLogFile
    {
        public DirectoryInfo Directory { get; internal set; }
        public string Name { get; internal set; }
    }
}
