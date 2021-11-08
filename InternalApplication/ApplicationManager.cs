using System;
using System.Collections.Generic;
using Bugtracker.Configuration;

namespace Bugtracker.InternalApplication
{

    /// <summary>
    /// This class is responsible for the application 
    /// handling and storing logical Applications as Objects in the form of lists. 
    /// </summary>
    ///
    class ApplicationManager
    {
        /// <summary>
        /// A list of all managed Applications
        /// </summary>
        private List<Application> applications;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationManager()
        {
            applications = ConfigHandler.GetSpecifiedApplications();
        }

        public void Init()
        {
            //get all pre configured applications from config.xml
            applications = ConfigHandler.GetSpecifiedApplications();
        }

        public void AddApplication(Application app)
        {
            applications.Add(app);
        }

        public List<Application> GetApplications()
        {
            return applications;
        }

        public List<string> GetApplicationNames()
        {
            List<string> appNames = new List<string>();
            foreach (Application app in applications)
            {
                appNames.Add(app.Name);
            }
            return appNames;
        }

        public override string ToString()
        {
            string applicationList = "";

            foreach (Application a in applications)
            {
                applicationList += a.ToString() + Environment.NewLine;
            }

            return applicationList;
        }

        internal Application GetApplicationByName(string appNameParameter)
        {
            foreach (Application app in applications)
            {
                if (app.Name == appNameParameter)
                    return app;
            }

            return null;
        }
    }



}
