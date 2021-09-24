using System;

namespace Bugtracker
{
	/// <summary>
	/// Enumarator specifieing the Actions you can take with
	/// an Application Object in the command line
	/// </summary>
	public enum ApplicationAction
    {
		ChangeName,
		ChangeExe,
		ChangeStandard,
		ChangeShowSpec
    }
	public class Application
	{
		public enum ShowAppSpecifier
		{
			onExist
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
		#endregion

		public Application()
		{

		}

        public override string ToString()
        {
			return "Name: " + this.Name + Environment.NewLine +
				   "Executable: " + this.ExecutableLocation + Environment.NewLine +
				   "Enabled: " + this.Enabled + Environment.NewLine +
				   "Standard: " + this.IsStandard + Environment.NewLine +
				   "Show: " + this.ShowSpecifier + Environment.NewLine;
        }
    }
}
