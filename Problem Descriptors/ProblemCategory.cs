using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugtracker.Problem_Descriptors
{
    public class ProblemCategory
    {
        public string Name { get; set; }
        public List<string> Descriptions { get; set; }
        public List<InternalApplication.Application> SelectedApplications { get; set; }
        public bool SelectAllApplications { get; set; }
        public bool SelectScreenshot { get; set; }

        public ProblemCategory()
        {
            SelectAllApplications = false;
            SelectScreenshot = false;

            Descriptions = new List<string>();
            SelectedApplications = new List<InternalApplication.Application>();
        }
    }
}
