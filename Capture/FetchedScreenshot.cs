using System.IO;

namespace Bugtracker.Capture
{
    class FetchedScreenshot
    {
        public DirectoryInfo Directory { get; internal set; }
        public string Name { get; internal set; }
    }
}
