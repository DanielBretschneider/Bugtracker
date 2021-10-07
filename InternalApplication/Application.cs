using System;

namespace Bugtracker.InternalApplication
{
	/// <summary>
	/// Enumarator specifieing the Actions you can take with
	/// an Application Object in the command line
	/// </summary>
	public enum ApplicationAction
    {
		name,
		exe,
		standard,
		showspec
    }

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
		#endregion

		public Application()
		{

		}

        public override string ToString()
        {
			return "Name: " + this.Name + "\t \t" +
				   "Executable: " + this.ExecutableLocation + "\t" +
				   "Enabled: " + this.Enabled + "\t" +
				   "Standard: " + this.IsStandard + "\t" +
				   "Show: " + this.ShowSpecifier;
        }

        internal static ApplicationAction GetAppActionPerString(string actionParameter)
        {
			ApplicationAction appAction;
			if (Enum.TryParse(actionParameter, out appAction))
				return appAction;
			else
				throw new CommandActionDoesNotExistEception("", "application", actionParameter);
		}
    }
}
