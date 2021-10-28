using Bugtracker.Logging;
using System.Collections.Generic;

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
