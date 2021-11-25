namespace Bugtracker.Globals_and_Information
{
    /// <summary>
    /// This Class stores all constants and global variables needed. The variables
    /// are readonly.
    /// </summary>
    class Globals
    {
        /*
         * TEXT SPECIFIC GLOBALS
         */

        /// <summary>
        /// End of Line Character
        /// </summary>
        public const string EOL_CHARACTER = "\n";

        /// <summary>
        /// space
        /// </summary>
        public const char SPACE_CHARACTER = ' ';

        /*
         * FILE PATHS
         */

        /// <summary>
        /// APPLICATION_PATH stores the path to the bugtracker directory
        /// </summary>
        public const string APPLICATION_DIRECTORY = @"C:\Bugtracker\";

        /// <summary>
        /// Temp folder where the bugtrack files will be gathered, zipped and sent
        /// out. BT Files + .zip files will be automatically deleted when sent out
        /// </summary>
        public const string TMP_DIRECTORY = APPLICATION_DIRECTORY + @"tmp\";

        /// <summary>
        /// LOG_FILE_PATH stores the absolute path to bugtracker.log
        /// </summary>
        public const string LOG_FILE_PATH = APPLICATION_DIRECTORY + "bugtracker.log";

        /// <summary>
        /// CONFIG_FILE_PATH holds the path where the configuration xml file is stored
        /// The 'x' on the end of filename can be changed into a combination of pc name and 
        /// domain.
        /// </summary>
        public const string LOCAL_CONFIG_FILE_PATH = APPLICATION_DIRECTORY + "bugtracker_config_startup.xml";

        /// <summary>
        /// CONFIG_FILE_PATH holds the path where the configuration xml files are stored
        /// The 'x' on the end of filename can be changed into a combination of pc name and 
        /// domain.
        /// </summary>
        public const string LOCAL_CONFIG_FILES_PATH = APPLICATION_DIRECTORY + "\\configs";


        /// <summary>
        /// CONFIG_FILE_PATH holds the path where the configuration xml files are stored
        /// The 'x' on the end of filename can be changed into a combination of pc name and 
        /// domain.
        /// </summary>
        public const string LOCAL_PLUGIN_FILES_PATH = APPLICATION_DIRECTORY + "\\plugins";

        /// <summary>
        /// DEFAULT_BUGTRACKER_SERVER holds the default value where the file have to be copied to
        /// </summary>
        public const string DEFAULT_BUGTRACKER_SERVER = @"\\10.74.10.100\bugTracker";

        /// <summary>
        /// Image file format of screenshots, jpg by default
        /// </summary>
        public const string SCREENSHOT_FILE_FORMAT = ".JPG";

        /// <summary>
        /// Number of lines that should be saved in log file
        /// </summary>
        public const int MAX_LINES_OF_LOG_FILE = 2000;
    }
}
