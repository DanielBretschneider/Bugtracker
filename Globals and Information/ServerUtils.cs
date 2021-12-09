using Bugtracker.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Bugtracker.Globals_and_Information
{
    public static class ServerUtils
    {
        private static readonly RunningConfiguration runningConfiguration = RunningConfiguration.GetInstance();
        public static EventHandler CheckedServerStatus;

        /// <summary>
        /// Method to test if server path still exists
        /// </summary>
        /// <returns>Returns DateTime of last connection-test, return 0,0,0 DateTime when not connected.</returns>
    }

    public class Server
    {
        /// <summary>
        /// 
        /// </summary>
        public ServerStatus ServerStatus { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public string ServerPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CheckInterval { get; set; }
        private static System.Timers.Timer aTimer;
        private readonly Ping Pinger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverPath"></param>
        public Server(string serverPath)
        {
            this.ServerPath = serverPath;

            Pinger = new Ping();
        }

        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(CheckInterval);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            try
            {
                PingReply reply = Pinger.Send(ServerPath);
                if(reply.Status == IPStatus.Success)
                    ServerStatus = ServerStatus.Available;
            }
            catch
            {
                ServerStatus = ServerStatus.NotAvailable;
            }
            finally
            {
                Pinger.Dispose();
            }
        }
    }
}
