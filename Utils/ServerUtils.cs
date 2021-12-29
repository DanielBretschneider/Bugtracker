using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Timers;
using Bugtracker.Configuration;

namespace Bugtracker.Utils
{
    public static class ServerUtils
    {
        private static readonly RunningConfiguration runningConfiguration = RunningConfiguration.GetInstance();
    }

    public class Server
    {
        public static event EventHandler CheckedServerStatus;
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
        private static System.Timers.Timer _aTimer;
        private readonly Ping pinger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverPath"></param>
        public Server(string serverPath)
        {
            pinger = new Ping();
            this.ServerPath = serverPath;

            SetTimer();

            

        }

        private void SetTimer()
        {
            _aTimer = new System.Timers.Timer(5000);
            _aTimer.Elapsed += OnTimedEvent;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                PingReply reply = pinger.Send(ServerPath);
                if(reply.Status == IPStatus.Success)
                    ServerStatus = ServerStatus.Available;
            }
            catch
            {
                ServerStatus = ServerStatus.NotAvailable;
            }

            if(CheckedServerStatus != null)
                CheckedServerStatus?.Invoke(null,null);
        }
    }
}
