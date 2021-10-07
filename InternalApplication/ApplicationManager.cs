using Bugtracker.GlobalsInformation;
using System;
using System.Collections.Generic;

namespace Bugtracker.InternalApplication
{
    public enum ApplicationManagerAction
    {
        list = 0,       //lists all applications of the running-configuration
        add = 1,        //adds a new application to the running-configuration
        remove = 2,     //removes application from running-configuration
        installed = 3,  //returns true if given application is installed
        write = 4,       //writes all changes in running-configuration made to applications to .xml
        help = 99       //returns help message
    }

    /// <summary>
    /// This class is resonsible for the application 
    /// handling and storing logical Applications as Objects in the form of lists. 
    /// </summary>
    ///
    class ApplicationManager : Singleton<ApplicationManager>
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

        public List<Application> GetApplications()
        {
            return applications;
        }

        public List<string> GetApplicationNames()
        {
            List<string> appNames = new List<string>();
            foreach(Application app in applications)
            {
                appNames.Add(app.Name);
            }
            return appNames;
        }

        public override string ToString()
        {
            string applicationList = "";

            foreach(Application a in applications)
            {
                applicationList += a.ToString() + Environment.NewLine;
            }

            return applicationList;
        }

        internal Application GetApplicationByName(string appNameParameter)
        {
            List<string> appNames = GetApplicationNames();
            
            foreach(Application app in applications)
            {
                if(app.Name == appNameParameter)
                {
                    return app;
                }
                else
                {
                    throw new ApplicationNotFoundException(appNameParameter);
                }
            }

            throw new NotImplementedException();
        }

        internal ApplicationManagerAction GetAppManagerActionPerString(string actionParameter)
        {
            ApplicationManagerAction appManAction;
            if (Enum.TryParse(actionParameter, out appManAction))
                return appManAction;
            else
                throw new CommandActionDoesNotExistEception();
        }
    }



}
