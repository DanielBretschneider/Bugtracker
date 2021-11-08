﻿using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Bugtracker.Logging;

namespace Bugtracker.Globals_and_Information
{
    class PCInfo
    {
        /// <summary>
        /// constructor
        /// </summary>
        public PCInfo()
        {
            Logger.Log("New PCinfo object was created.", (LoggingSeverity)2);
        }


        /// <summary>
        /// Returns hostname of device
        /// </summary>
        /// <returns></returns>
        public string GetHostname()
        {
            Logger.Log("GetHostname() executed", (LoggingSeverity)2);
            return Dns.GetHostName();
        }

        /// <summary>
        /// Returns the domain name
        /// </summary>
        /// <returns></returns>
        public string GetDomainName()
        {
            Logger.Log("GetDomainName() executed", (LoggingSeverity)2);
            return System.Environment.UserDomainName;
        }

        /// <summary>
        /// Returns IP address of device
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
            Logger.Log("GetIPAddress() executed", (LoggingSeverity)2);
            // host
            var host = Dns.GetHostEntry(Dns.GetHostName());

            // multiple results possible
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            // in case of error throw exception
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        /// <summary>
        /// Returns MAC address of device
        /// </summary>
        /// <returns></returns>
        public string GetMACAddress()
        {
            // get mac address
            var mac_address =
                    (from nic in NetworkInterface.GetAllNetworkInterfaces()
                     where nic.OperationalStatus == OperationalStatus.Up
                     select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();


            // return mac as string
            return mac_address.ToString();
        }

        /// <summary>
        /// Returns name of current user
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserName()
        {
            // get current user name
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }


        /// <summary>
        /// Get time and date of last boot up 
        /// from WMI
        /// </summary>
        /// <returns></returns>
        public string GetLastBootUpTime()
        {
            // time and date will be stored here
            string txtDate = "";
            string txtTime = "";

            // define a select query
            SelectQuery query =
                new SelectQuery(@"SELECT LastBootUpTime FROM Win32_OperatingSystem
                WHERE Primary='true'");

            // create a new management object searcher and pass it
            // the select query
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(query);

            // get the datetime value and set the local boot
            // time variable to contain that value
            foreach (ManagementObject mo in searcher.Get())
            {
                var dtBootTime =
                    ManagementDateTimeConverter.ToDateTime(
                        mo.Properties["LastBootUpTime"].Value.ToString());

                // display the start time and date
                txtDate = dtBootTime.ToLongDateString();
                txtTime = dtBootTime.ToLongTimeString();
            }

            // return time and date formatted
            return (txtDate + " - " + txtTime);
        }


        /// <summary>
        /// Summary on pc contains, hostname
        /// ip, mac, domain name and currently logged
        /// in user
        /// </summary>
        /// <returns></returns>
        public string GetPCInformationSummary()
        {
            StringBuilder info = new StringBuilder();

            try
            {
                info.AppendLine("Hostname:\t" + GetHostname());
                info.AppendLine("Domain:\t\t" + GetDomainName());
                info.AppendLine("User: \t\t" + GetCurrentUserName());
                info.AppendLine("IP-Addr.:\t" + GetIPAddress());
                info.AppendLine("MAC-Addr.:\t" + GetMACAddress());
                info.AppendLine("Startup time: \t" + GetLastBootUpTime());
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, 0);
            }

            return info.ToString();
        }

    }
}
