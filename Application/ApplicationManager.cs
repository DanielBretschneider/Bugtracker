using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker
{
    public enum ApplicationManagerAction
    {
        ListAll,
        Add,
        IsApplicationInstalled
    }


    /// <summary>
    /// This class is resonsible for the application 
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
        public ApplicationManager(List<Application> applications)
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

        public List<Application> GetApplications()
        {
            return applications;
        }
    }



}
