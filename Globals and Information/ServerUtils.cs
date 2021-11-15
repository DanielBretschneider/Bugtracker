using Bugtracker.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker.Globals_and_Information
{
    internal static class ServerUtils
    {
        private static readonly RunningConfiguration runningConfiguration = RunningConfiguration.GetInstance();
        public static EventHandler CheckedServerStatus;

        /// <summary>
        /// Method to test if server path still exists
        /// </summary>
        /// <returns>Returns DateTime of last connection-test, return 0,0,0 DateTime when not connected.</returns>
        public static ServerStatus GetServerStatus()
        {
            CheckedServerStatus?.Invoke(null, new EventArgs());

            if (Directory.Exists(runningConfiguration.ServerPath))
                return ServerStatus.Available;
            else
                return ServerStatus.NotAvailable;
            
        }
    }
}
