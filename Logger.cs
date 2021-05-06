using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker
{
    class Logger
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Logger()
        {
            initializeLogging();
        }


        /// <summary>
        /// Check if log file exists
        /// or has to be created
        /// </summary>
        public void initializeLogging()
        {
            // if file doesn't exist create file 
            if (!File.Exists(Globals.LOG_FILE_PATH))
            {
                // create application directory 
                DirectoryInfo di = Directory.CreateDirectory(Globals.APPLICATION_DIRECTORY);
                File.Create(Globals.LOG_FILE_PATH).Dispose();
            }

            // check if config file exists
            checkConfigFile();
        }


        /// <summary>
        /// This method checks if a config file already exists in local
        /// if so, nothing will be done. If not, then the default configuration
        /// will be copied to specified location.
        /// </summary>
        public void checkConfigFile()
        {
            // check if local config exists
            if (!File.Exists(Globals.CONFIG_FILE_PATH))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(Globals.CONFIG_FILE_PATH))
                {
                    // write xml content to file
                    sw.WriteLine(Globals.CONFIG_FILE_CONTENT);
                }
            }
        }


        /// <summary>
        /// To log, or not to log.
        /// </summary>
        /// <returns></returns>
        public bool isLogEnabled()
        {
            // value of target aka site/ip to be pinged
            bool log = false;

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName == "log")
                        {
                            // get value of target
                            string value = reader.GetAttribute("enabled");

                            // if visible == "true" getHostname
                            if (value.Equals("true"))
                            {
                                log = true;
                            }
                        }
                    }
                }
            }

            return log;
        }


        /// <summary>
        /// log message with priority
        /// 0 = ERRROR
        /// 1 = WARNING
        /// 2 = INFO
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="priority"></param>
        public void log(string msg, int priority)
        {
            // get local date and time
            DateTime localDate = DateTime.Now;

            // de-DE datetime
            string dateAndTime = localDate.ToString("dd.MM.yyyy HH:mm:ss");

            // full message
            string fmsg = "";

            // checking if logging is enabled
            if (isLogEnabled() == false)
                return;

            // different message, depends on priority
            switch (priority)
            {
                case 0:
                    fmsg = "[ERROR][" + dateAndTime + "]: " + msg;
                    appendToFile(fmsg);
                    break;

                case 1:
                    fmsg = "[WARNING][" + dateAndTime + "]: " + msg;
                    appendToFile(fmsg);
                    break;

                case 2:
                    fmsg = "[INFO][" + dateAndTime + "]: " + msg;
                    appendToFile(fmsg);
                    break;
            }
        }


        /// <summary>
        /// Append message to log file
        /// </summary>
        /// <param name="msg"></param>
        public void appendToFile(String msg)
        {
            // if logging is enabled in config file, log
            if (true)
            {
                using (StreamWriter writer = File.AppendText(Globals.LOG_FILE_PATH))
                {
                    writer.WriteLine(msg);
                }
            }
        }
    }
}
