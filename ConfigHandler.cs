using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bugtracker 
{ 

    /// <summary>
    /// This class is only here to handle all the XML-Magic
    /// </summary>
    class ConfigHandler
    {

        /// <summary>
        /// Default Constructor,
        /// does nothing
        /// </summary>
        public ConfigHandler()
        {
            //nop
        }

        
        /// <summary>
        /// Returns number of applications specified in bugtracker Config
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfApplications()
        {
            // value of target aka site/ip to be pinged
            int appCount = 0;

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName == "application")
                        {
                            appCount++;
                        }
                    }
                }
            }

            return appCount;
        }


        /// <summary>
        /// Return all names of applications specified in the logfile
        /// specified in the 
        /// </summary>
        /// <returns></returns>
        public List<string> GetSpecifiedApplicationNames()
        {
            // string list holds the names of the applications
            List<string> applicationNames = new List<string>();

            // start reading autostart.config.xml
            using (XmlReader reader = XmlReader.Create(Globals.CONFIG_FILE_PATH))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.LocalName == "application")
                        {
                            //applicationNames.Add(reader.LocalName.);
                        }
                    }
                }
            }

            return applicationNames;
        }

    }
}
