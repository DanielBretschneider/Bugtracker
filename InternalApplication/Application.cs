using Bugtracker.Logging;
using System.Collections.Generic;
using System.IO;

namespace Bugtracker.InternalApplication
{
    class Application
	{
		public enum ShowAppSpecifier
		{
			onExist,
			show,
			hide
		}


		#region Properties
		/// <summary>
		/// The Name specifies the name of the Application
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Specifies the location of the executable as a string 
		/// </summary>
		public string ExecutableLocation { get; set; }

		/// <summary>
		/// Specifies if Application is enabled in Bugtracker
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Specifies if its the Standard Application
		/// </summary>
		public bool IsStandard { get; set; }

		public bool IsInstalled
        {
			get
            {
				System.Diagnostics.Debug.WriteLine("File check exist: "  + Name + ": " + (LogFiles[0].Path != "" && Directory.Exists(LogFiles[0].Path)));
				if (LogFiles[0].Path != "" && Directory.Exists(LogFiles[0].Path))
					return true;
				else
					return false;
            }
        }

		/// <summary>
		/// Specifier if show "onExist" -> show Application in when installed on PC
		/// </summary>
		public ShowAppSpecifier ShowSpecifier { get; set; }

		/// <summary>
		/// 
		/// </summary>
        public List<Log> LogFiles { get; set; }

        #endregion

        public Application()
		{
			LogFiles = new List<Log>();
		}

        public override string ToString()
        {
			return "Name: " + this.Name + "\t \t" +
				   "Executable: " + this.ExecutableLocation + "\t" +
				   "Enabled: " + this.Enabled + "\t" +
				   "Standard: " + this.IsStandard + "\t" +
				   "Show: " + this.ShowSpecifier;
        }
    }
}
