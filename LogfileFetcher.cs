using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bugtracker
{
    class LogfileFetcher
    {

        /// <summary>
        /// LogFileFetcher() - default Constructor
        /// </summary>
        public LogfileFetcher()
        {
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string BuildDestinationPath()
        {
            // TODO
            string destinationPath = "";

            return destinationPath;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private int GetLineCount(string filepath)
        {
            // line count of file
            int lineCount = 0;
            
            // count lines
            using (var reader = File.OpenText(@"C:\file.txt"))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }

            // return counter variable
            return lineCount;
        }


        /// <summary>
        /// Copies file 
        /// If file has more lines than n = MAX_LINES_OF_LOG_FILE
        /// than only the last n lines will be stored in bugtracker zip
        /// </summary>
        public void CopyFile(string pathToLogFile, string destionPath)
        {
            // check lines of log file
            if (GetLineCount(pathToLogFile) >= Globals.MAX_LINES_OF_LOG_FILE)
            {
                
            }
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
        public bool FetchLog(string logfilePath)
        {
            // status of fetch
            bool fetch_status = false;

            // build folder name
            string destinationPath = BuildDestinationPath();

            // check if file exists
            if (File.Exists(logfilePath))
            {
                // copy file to desired destination
                CopyFile(logfilePath, destinationPath);
            }

            // return status
            return fetch_status;
        }


    }
}
