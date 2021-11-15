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
        public List<Application> Applications { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ApplicationManager()
        {
            Applications = new List<Application>();
        }

        public void AddApplication(Application app)
        {
            Applications.Add(app);
        }

        public List<Application> GetApplications()
        {
            return Applications;
        }

        public List<string> GetApplicationNames()
        {
            List<string> appNames = new List<string>();
            foreach (Application app in Applications)
            {
                appNames.Add(app.Name);
            }
            return appNames;
        }

        public override string ToString()
        {
            string applicationList = "";

            foreach (Application a in Applications)
            {
                applicationList += a.ToString() + Environment.NewLine;
            }

            return applicationList;
        }

        internal Application GetApplicationByName(string appNameParameter)
        {
            foreach (Application app in Applications)
            {
                if (app.Name == appNameParameter)
                    return app;
            }

            return null;
        }
    }



}
