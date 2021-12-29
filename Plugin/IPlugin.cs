

namespace Bugtracker.Plugin
{
    /// <summary>
    /// Plugin Interface, should be implemented when creating a plugin type
    /// </summary>
    public interface IPlugin
    {
         string Name { get; }
         string Version { get; }  
         string Author { get; }   
         void OnLoad();
    }
}
