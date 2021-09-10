using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker
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
        public const string LOG_FILE_PATH = APPLICATION_DIRECTORY + "bugtracker_log_x.log";

        /// <summary>
        /// CONFIG_FILE_PATH holds the path where the configuration xml file is stored
        /// The 'x' on the end of filename can be changed into a combination of pc name and 
        /// domain.
        /// </summary>
        public const string CONFIG_FILE_PATH = APPLICATION_DIRECTORY + "bugtracker_config_x.xml";

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

        /*
         * CONTENT CONFIGURATION XML
         */

        /// <summary>
        /// CONFIG_FILE_CONTENT stores the defautl bugtracker configuration
        /// </summary>
        public const string CONFIG_FILE_CONTENT = @"<?xml version='1.0' encoding='utf-8'?>
<!-- DEFAULT AUTOSTART CONFIG -->
<configuration>
  <!-- ACTIVE or DEACTIVATE LOGGING and DEBUG INFORMATION -->
  <debug>
    <log enabled='false' />
  </debug>
  <!-- NOTIFICATION SERVICES -->
  <notification>
    <enablenotification enabled='true' />
    <queue name='' group='' location='' />
  </notification>
  <!-- Settings for UI component visibility-->
  <visibility>
    <hostname visible='true' />
    <programvisibility shownotinstalled='false' />
    <!-- 1000 = 10 seconds, 500 = 20 seconds ... -->
    <progressbar timer='1000' />
  </visibility>
  <!-- programs to be opened-->
  <programs>
    <program name='XR Manage' start='false' execute='XR RIS manage.lnk' path='C:\Users\Public\Desktop' processname='XR5.exe' />
    <program name='XR Timer' start='false' execute='XR RIS timer.lnk' path='C:\Users\Public\Desktop' processname='XrTimer5.exe' />
    <program name='XR Importer' start='false' execute='XR RIS Importer.lnk' path='C:\Users\Public\Desktop' processname='XRHISImporter.exe' />
    <program name='Commander IP7' start='false' execute='XR RIS Commander.lnk' path='C:\Users\Public\Desktop' processname='XRRisCommander.exe' />
    <program name='Commander IP8' start='false' execute='Xr RIS Commander (für IP8).lnk' path='C:\Users\xrsec\Desktop' processname='XRRisCommander.exe' />
    <program name='ImagePro 7' start='false' execute='XRPacsImageproSecureStart.exe.lnk' path='C:\Users\xrsec\Desktop' processname='XrPacsImagePro.exe' />
    <program name='ImagePro 8' start='false' execute='ImagePro 8.lnk' path='C:\Users\xrsec\Desktop' processname='IPWorkstation.exe' />
    <program name='Chrome' start='false' execute='chrome.exe' path='C:\Program Files (x86)\Google\Chrome\Application' processname='chrome.exe' />
  </programs>
  <!-- CHECKS-->
  <checkconnectivity>
    <target value='www.google.at' name='internet' />
    <target value='server' name='server' />
    <target value='' name='gina' />
  </checkconnectivity>
</configuration>";


    }
}
