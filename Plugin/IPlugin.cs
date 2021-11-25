

namespace Bugtracker.Plugin
{
    /// <summary>
    /// Plugin Interface, should be implemented when creating a plugin type
    /// </summary>
    public interface IPlugin
    {
        public string Name { get; }
        public string Version { get; }  
        public string Author { get; }   
        public void OnLoad();
    }
}
