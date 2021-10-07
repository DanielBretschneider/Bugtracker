namespace Bugtracker.Console
{
    public interface ICommand
    {
        /// <summary>
        /// Main Logic of Command
        /// </summary>
        string ExecuteAction();
    }
}
