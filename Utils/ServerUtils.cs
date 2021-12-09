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
        public static EventHandler CheckedServerStatus;


        public static string IsTerminalServer()
        {
            //create a management scope object
            ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\CIMV2\\TerminalServices");

            //create object query
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_TerminalServiceSetting");

            //create object searcher
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(scope, query);

            //get a collection of WMI objects
            ManagementObjectCollection queryCollection = searcher.Get();

            foreach (ManagementObject m in queryCollection)
            {
                // access properties of the WMI object
                return "Terminal Server enabled: " + m["AllowTSConnections"];
            }

            return "";
        }
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
        private static System.Timers.Timer _aTimer;
        private readonly Ping pinger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverPath"></param>
        public Server(string serverPath)
        {
            this.ServerPath = serverPath;

            pinger = new Ping();
        }

        private void SetTimer()
        {
            _aTimer = new System.Timers.Timer(CheckInterval);
            _aTimer.Elapsed += OnTimedEvent;
            _aTimer.AutoReset = true;
            _aTimer.Enabled = true;
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
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
            finally
            {
                pinger.Dispose();
            }
        }
    }
}
