using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using Bugtracker.Attributes;
using Bugtracker.Configuration;
using Bugtracker.Logging;

namespace Bugtracker.Globals_and_Information
{
    public class PCInfo
    {
        private protected RunningConfiguration _configuration;
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
        [Key("hostname")]
        public static string Hostname
        {
            get
            {
                return Dns.GetHostName();
            }
            
        }

        [Key("clientname")]
        public static string Clientname
        {
            get
            {
                Logger.Log("GetHostname() executed", (LoggingSeverity)2);

                if(IsRemoteSession)
                {
                    return System.Environment.GetEnvironmentVariable("CLIENTNAME");
                }
                else
                {
                    return Hostname;
                }
            }
        }

        [Key("remoteSession")]
        public static bool IsRemoteSession
        {
            get => System.Windows.Forms.SystemInformation.TerminalServerSession;
        }

        /// <summary>
        /// Returns the domain name
        /// </summary>
        /// <returns></returns>
        [Key("domainName")]
        public static string DomainName
        {
            get
            {
                Logger.Log("GetDomainName() executed", (LoggingSeverity)2);
                return System.Environment.UserDomainName;
            }

        }

        /// <summary>
        /// Returns IP address of device
        /// </summary>
        /// <returns></returns>
        [Key("ipAddress")]
        public static string IPAddress
        {
            get
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
        }

        /// <summary>
        /// Returns MAC address of device
        /// </summary>
        /// <returns></returns>
        [Key("macAddress")]
        public static string MACAddress
        {
            get
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

        }

        /// <summary>
        /// Returns name of current user
        /// </summary>
        /// <returns></returns>
        [Key("userName")]
        public static string CurrentUserName
        {
            get
            {
                // get current user name
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
        }


        /// <summary>
        /// Get time and date of last boot up 
        /// from WMI
        /// </summary>
        /// <returns></returns>
        [Key("bootTime")]
        public static string LastBootTime
        {
            get
            {
                // time and date will be stored here
                string txtDate = "";
                string txtTime = "";

                // define a select query
                SelectQuery query =
                    new(@"SELECT LastBootUpTime FROM Win32_OperatingSystem
                WHERE Primary='true'");

                // create a new management object searcher and pass it
                // the select query
                ManagementObjectSearcher searcher =
                    new(query);

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
        }


        /// <summary>
        /// Summary on pc contains, hostname
        /// ip, mac, domain name and currently logged
        /// in user
        /// </summary>
        /// <returns></returns>
        public static string Summary()
        {
            StringBuilder info = new();
            
            try
            {
                info.AppendLine("Hostname:\t"       + Hostname);
                info.AppendLine("Domain:\t\t"       + DomainName);
                info.AppendLine("User: \t\t"        + CurrentUserName);
                info.AppendLine("IP-Addr.:\t\t"     + IPAddress);
                info.AppendLine("MAC-Addr.:\t"      + MACAddress);
                info.AppendLine("Startup time: \t"  + LastBootTime);
                info.AppendLine("Remote Session\t" +  IsRemoteSession);
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, 0);
            }

            return info.ToString();
        }

    }
}
