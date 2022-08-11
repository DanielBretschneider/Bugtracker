using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Bugtracker.Configuration;
using Bugtracker.Logging;

namespace Bugtracker.Plugin
{
    public static class PluginManager
    {
        public static void Load()
        {
            List<string> pluginFiles = new List<string>();

            pluginFiles.AddRange(Directory.GetFiles(Globals_and_Information.Globals.GetFittingPluginFilesPath(), "*.dll"));

            foreach (String pluginFile in pluginFiles)
            {
                Type objectType = null;

                System.Diagnostics.Debug.WriteLine("Trying to Load Plugin: " + pluginFile);

                try
                {
                    Assembly asm = Assembly.LoadFrom(pluginFile);

                    if(asm != null)
                    {
                        var type = typeof(IPlugin);

                        var types = asm.GetTypes().Where(p => type.IsAssignableFrom(p));

                        foreach (var plugin in types)
                        {
                            System.Diagnostics.Debug.WriteLine("Loaded Plugin: " + plugin);

                            IPlugin ipl = (IPlugin)Activator.CreateInstance(plugin);
                            MethodInfo method = plugin.GetMethod("OnLoad");
                            method.Invoke(ipl, null);

                            Logger.Log("Loaded Plugin " + plugin, LoggingSeverity.Info);

                            RunningConfiguration.GetInstance().LoadedPlugins.Add(ipl);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Logger.Log("Error while loading plugin: " + ex.ToString(), Logging.LoggingSeverity.Error);
                }
            }
        }
    }
}
