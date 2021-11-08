using System;
using System.IO;
using Bugtracker.Configuration;
using Bugtracker.Globals_and_Information;

namespace Bugtracker.Logging
{
    public enum LoggingSeverity
    {
        //TODO: Remove Null
        Null = 99,
        Info = 3,
        Warning = 2,
        Error = 1
    }

    static class Logger
    {
        /// <summary>
        /// Check if log file exists
        /// or has to be created
        /// </summary>
        public static void InitializeLogging()
        {
            // if file doesn't exist create file 
            if (!File.Exists(Globals.LOG_FILE_PATH))
            {
                // create application directory 
                Directory.CreateDirectory(Globals.APPLICATION_DIRECTORY);
                File.Create(Globals.LOG_FILE_PATH).Dispose();
            }

            // check if config file exists
            Logger.CheckConfigFile();
        }


        /// <summary>
        /// return content of bugtrackerv2.config.xml
        /// 
        /// TODO: Change static path
        /// </summary>
        /// <returns></returns>
        public static string GetConfigFileContent()
        {
            // get content of config file
            string fileContent = File.ReadAllText(@"C:\Users\Daniel Bretschneider\source\repos\Bugtracker Version 2\bugtrackerv2.config.xml");

            // return content as string
            return fileContent;
        }


        /// <summary>
        /// This method checks if a config file already exists in local
        /// if so, nothing will be done. If not, then the default configuration
        /// will be copied to specified location.
        /// </summary>
        public static void CheckConfigFile()
        {
            // check if local config exists
            if (!File.Exists(Globals.CONFIG_FILE_PATH))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(Globals.CONFIG_FILE_PATH))
                {
                    // write xml content to file
                    sw.WriteLine(GetConfigFileContent());
                }
            }
        }

        /// <summary>
        /// log message with priority
        /// 0 = ERRROR
        /// 1 = WARNING
        /// 2 = INFO
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="priority"></param>
        public static void Log(string msg, LoggingSeverity loggingSeverity)
        {
            // get local date and time
            DateTime localDate = DateTime.Now;

            // de-DE datetime
            string dateAndTime = localDate.ToString("dd.MM.yyyy HH:mm:ss");

            // full message
            string fmsg;

            // checking if logging is enabled
            //TODO: Fix -> Singleton -> Stackoverflow RunningConfiguration.GetInstance().LoggerEnabled
            if (ConfigHandler.IsLoggingEnabled() == false)
                return;

            // different message, depends on priority
            switch (loggingSeverity)
            {
                case LoggingSeverity.Error:
                    fmsg = "[ERROR][" + dateAndTime + "]: " + msg;
                    AppendToFile(fmsg);
                    break;

                case LoggingSeverity.Warning:
                    fmsg = "[WARNING][" + dateAndTime + "]: " + msg;
                    AppendToFile(fmsg);
                    break;

                case LoggingSeverity.Info:
                    fmsg = "[INFO][" + dateAndTime + "]: " + msg;
                    AppendToFile(fmsg);
                    break;
            }
        }


        /// <summary>
        /// Append message to log file
        /// </summary>
        /// <param name="msg"></param>
        public static void AppendToFile(String msg)
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
