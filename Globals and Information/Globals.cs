using Bugtracker.Configuration;

namespace Bugtracker.Globals_and_Information
{
    /// <summary>
    /// This Class stores all constants and global variables needed. The variables
    /// are readonly.
    /// </summary>
    public class Globals
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

        public static string APPLICATION_DIRECTORY = (string) RunningConfiguration.GetInstance().VariableManager.VariableDictionary["targetdir"].value;

        public static string TMP_DIRECTORY = APPLICATION_DIRECTORY + @"tmp\";

        public static readonly string LOG_FILE_PATH = APPLICATION_DIRECTORY + "bugtracker.log";

        public static readonly string LOCAL_STARTUP_CONFIG_FILE_PATH = APPLICATION_DIRECTORY + "bugtracker_config_startup.xml";

        public static readonly string LOCAL_CONFIG_FILES_PATH = APPLICATION_DIRECTORY + "configs";

        public static readonly string LOCAL_BLACKHOLE_FODLER_PATH = APPLICATION_DIRECTORY + "blackhole";


        public static readonly string LOCAL_PLUGIN_FILES_PATH = APPLICATION_DIRECTORY + "plugins";

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
