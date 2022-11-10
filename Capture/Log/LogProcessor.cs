using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Bugtracker.Configuration;
using static System.Environment;

namespace Bugtracker.Capture.LogProcessing
{
    public static class LogProcessor
    {
        //private List<InternalApplication.Application> targetApplications;
        private static readonly string bugtrackerFolderPath = RunningConfiguration.GetInstance().NewestBugtrackerFolder.FullName;

        public static event EventHandler RenamedLogs;
        public static event EventHandler DeletedLogs;
        public static event EventHandler FetchedLogs;

        /// <summary>
        /// Returns a list of paths for each log file existing
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, (InternalApplication.Application application, Logging.Log log)> InitLogFilesFromAllTargetedApplications(List<InternalApplication.Application> targetApplications)
        {
            Dictionary<string, (InternalApplication.Application application
                , Logging.Log log)> allLogFiles = new();

            foreach (InternalApplication.Application app in targetApplications)
            {
                foreach (Logging.Log logF in app.LogFiles)
                {
                    System.Diagnostics.Debug.WriteLine("Path: " + logF.Path);
                    System.Diagnostics.Debug.WriteLine("Filename: " + logF.Filename);

                    FileInfo[] allFiles = null;
                    FileInfo newestFile = null;

                    DirectoryInfo logDir = new(logF.Path);

                    if (Directory.Exists(logF.Path))
                    {
                        if (logF.Find == Logging.Log.LogFindSpecifier.NEW)
                        {
                            if(logDir.GetFiles(logF.Filename).Length > 0)
                                newestFile = logDir.GetFiles(logF.Filename).OrderByDescending(f => f.LastWriteTime).First();
                        }
                        else
                        {
                            allFiles = logDir.GetFiles(logF.Filename);
                        }

                        if (logF.Find == Logging.Log.LogFindSpecifier.ALL)
                        {
                            foreach (FileInfo file in allFiles)
                            {
                                allLogFiles[file.FullName] = (app, logF);
                            }
                        }
                        else
                        {
                            if(newestFile != null) 
                                allLogFiles[newestFile.FullName] = (app, logF);
                        }

                    }
                }
            }

            return allLogFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string BuildDestinationPath()
        {
            // TODO Build Destionation Path
            string destinationPath = "";

            return destinationPath;
        }

        public static void DeleteAllTargeted(List<InternalApplication.Application> targetApplications)
        {
            Dictionary<string, (InternalApplication.Application application, Logging.Log log)> appLogs =
                InitLogFilesFromAllTargetedApplications(targetApplications);

            foreach (string log in appLogs.Keys)
            {
                File.Delete(log);
            }

            DeletedLogs?.Invoke(null, null);
        }

        public static void RenameAllTargeted(List<InternalApplication.Application> targetApplications)
        {
            Dictionary<string, (InternalApplication.Application application, Logging.Log log)> appLogs =
                InitLogFilesFromAllTargetedApplications(targetApplications);

            string newFilename;
            string date;
            string? filenameWithoutEx;

            foreach (string log in appLogs.Keys)
            {
                date = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss");
                filenameWithoutEx = Path.GetFileNameWithoutExtension(log);

                newFilename =  $"{filenameWithoutEx}[{date}].log";

                File.Move(log, $"{Path.GetDirectoryName(log)}\\{newFilename}");
            }

            RenamedLogs?.Invoke(null, null);
        }


        /// <summary>
        /// Copies file 
        /// If file has more lines than n = MAX_LINES_OF_LOG_FILE
        /// than only the last n lines will be stored in bugtracker zip
        /// </summary>
        public static void CopyFile(string pathToLogFile, Logging.Log log, string destionPath)
        {
            List<string> lines = new();

            using (var stream = new FileStream(pathToLogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using var reader = new StreamReader(stream, Encoding.UTF8);

                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            //if (!string.IsNullOrEmpty(log.Lines))
            //    lines.TakeLast(Int32.Parse(log.Lines));

            //var lines = !string.IsNullOrEmpty(log.Lines) ? File.ReadLines(pathToLogFile).TakeLast(Int32.Parse(log.Lines)) : File.ReadLines(pathToLogFile);

            File.WriteAllLines(destionPath + "\\" + Path.GetFileName(pathToLogFile), lines);

            System.Diagnostics.Debug.WriteLine("log file path: " + destionPath + "\\" + Path.GetFileName(pathToLogFile));
        }

        public static void FetchAllLogFiles(List<InternalApplication.Application> targetApplications)
        {
            Dictionary<string, (InternalApplication.Application application, Logging.Log log)> appLogs = InitLogFilesFromAllTargetedApplications(targetApplications);

            foreach (string log in appLogs.Keys)
            {
                appLogs.TryGetValue(log, out (InternalApplication.Application application, Logging.Log log) appAndLog);

                FetchLog(log, appAndLog);
            }

            FetchedLogs?.Invoke(null, null);
        }

        /// <summary>
        /// This procedure fetches the last 2000 
        /// (default value is MAX_LINES_OF_LOG_FILE) lines
        /// </summary>
        /// <param name="logfilePath"></param>
        /// <param name="destination"></param>
        /// <returns>
        /// 
        /// fetchstatus
        /// 0 = successfull
        /// 1 = failed
        /// </returns>
        public static bool FetchLog(string logfilePath, (InternalApplication.Application application, Logging.Log log) appAndLog)
        {
            //pre Fetch Program
            appAndLog.application.ExecutePreFetching();

            // status of fetch
            bool fetch_status = false;

            // check if file exists
            if (File.Exists(logfilePath))
            {
                fetch_status = true;
                // copy file to desired destination

                string path = bugtrackerFolderPath + "\\" + appAndLog.log.LocationType + "\\" + 
                              Path.GetDirectoryName(appAndLog.log.Path[3..]);
                
                Directory.CreateDirectory(path);
                CopyFile(logfilePath, appAndLog.log, path);
            }

            //post Fetch Program
            appAndLog.application.ExecutePostFetching();

            // return status
            return fetch_status;
        }
    }
}
