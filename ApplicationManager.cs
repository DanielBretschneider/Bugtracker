using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker_Version_2
{
    /// <summary>
    /// This class is resonsible for the application 
    /// handling. 
    /// </summary>
    class ApplicationManager
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationManager()
        {

        }


        /// <summary>
        /// This method trys to find a given file 
        /// (f.e. notepad.exe), if found than application
        /// is seen as installed -> return true
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsApplicationInstalled(string path)
        {

            // dummy return
            return false;
        }

    }


    /// <summary>
    /// This class cotains all the information 
    /// needed for one application.
    /// 
    /// RELEVANT INFORMATION ON OBJECT CREATION
    /// if application has multiple logfiles, create object like f.e.
    ///     Application application = new Application("appName", "appPath", "exefile.exe");
    ///     and add the log file afterwards via addLogFileLocation("/.../...");
    ///     
    /// if application has a single logfile then just write it into the constructor call
    ///     Application application = new Application("appName", "appPath", "exefile.exe", "logfilelocation.log");
    /// </summary>
    class Application
    {

        #region globals
        // Here are the class attributes stored

        /// <summary>
        /// Name of application
        /// </summary>
        private string applicationName = "";

        /// <summary>
        /// Stores the appication folder
        /// </summary>
        private string applicationPath = "";

        /// <summary>
        /// where is the executable file stored,
        /// normally we will find it somewhere in 
        /// the applicationPath
        /// </summary>
        private string executableFilePath = "";

        /// <summary>
        /// Most applications have one logfile, but there are certain 
        /// cases in which multiple logfiles are stored, so here's 
        /// a list that can store up to hundreds of logFilelocations
        /// </summary>
        private List<string> LogFileLocations = new List<string>();

        /// <summary>
        /// True if application has multiple logfiles
        /// </summary>
        private bool hasMultipleLogFiles = false;

        #endregion


        #region constructors
        // some space for the constructor

        /// <summary>
        /// Default Constructor with Parameters
        /// </summary>
        public Application(string applicationName, string applicationPath, string executableFilePath)
        {
            this.applicationName = applicationName;
            this.applicationPath = applicationPath;
            this.executableFilePath = executableFilePath;
            this.hasMultipleLogFiles = true;
        }

        /// <summary>
        /// Default Constructor with Parameters used for applicatio with only one logfile
        /// </summary>
        public Application(string applicationName, string applicationPath, string executableFilePath, string logFileLocation)
        {
            this.applicationName = applicationName;
            this.applicationPath = applicationPath;
            this.executableFilePath = executableFilePath;
            this.LogFileLocations.Add(logFileLocation);
            this.hasMultipleLogFiles = false;
        }


        #endregion


        #region getterAndSetter

        // Setter methods

        /// <summary>
        /// Setter methode for the application name
        /// </summary>
        /// <param name="applicationName"></param>
        public void SetApplicationName(string applicationName)
        {
            this.applicationName = applicationName;
        }


        /// <summary>
        /// Setter method for application path
        /// </summary>
        /// <param name="applicationPath"></param>
        public void SetApplicationPath(string applicationPath)
        {
            this.applicationPath = applicationPath;
        }


        /// <summary>
        /// Setter methode for the executable file path
        /// </summary>
        /// <param name="executableFilePath"></param>
        public void SetExecutableFilePath(string executableFilePath)
        {
            this.executableFilePath = executableFilePath;
        }

        // Getter methods

        /// <summary>
        /// Return application name as string
        /// </summary>
        /// <returns></returns>
        public string GetApplicatinName()
        {
            return this.applicationName;
        }


        /// <summary>
        /// Return application path as string
        /// </summary>
        /// <returns></returns>
        public string GetApplicationPath()
        {
            return this.applicationPath;
        }


        /// <summary>
        /// Returns path of executable file of 
        /// appliation
        /// </summary>
        /// <returns></returns>
        public string GetExecutableFilePath()
        {
            return this.executableFilePath;
        }

        /// <summary>
        /// Returns true if application has multiple log
        /// files
        /// </summary>
        /// <returns></returns>
        public bool HasMultipleLogFiles()
        {
            return this.hasMultipleLogFiles;
        }


        /// <summary>
        /// Returns complete List of log file locations
        /// </summary>
        /// <returns></returns>
        public List<string> GetLogFileLocations()
        {
            return this.LogFileLocations;
        }

        #endregion


        #region methods

        /// <summary>
        /// Adds an entry to List<string> LogFileLocations
        /// </summary>
        /// <param name="logFileLocation"></param>
        public void AddLogFileLocation(string logFileLocation)
        {
            // add file path to string list
            this.LogFileLocations.Add(logFileLocation);
        }

        /// <summary>
        /// Can only be executed if hasMultipleLogfiles is set to false!
        /// </summary>
        /// <returns></returns>
        public string GetLogFileLocation()
        {
            // check if multiple log files are setted
            if (hasMultipleLogFiles == true)
            {
                return "";
            }

            // since only one object is store return first element
            return this.LogFileLocations.First();
        }

        #endregion

    }
}
