﻿using Bugtracker.Globals_and_Information;
using Bugtracker.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bugtracker.Console
{
    /// <summary>
    /// Holds methods and values every command should inherit
    /// ex: GetHelp, returns string how to use given command
    /// </summary>
    class Command
    {
        public string               HelpMessage;
        public string               CommandName { get; set; }
        public string               CommandAlias { get; set; }
        public string[]             CommandRequiredAttributes { get; set; }
        public string[]             CommandOptionalAttributes { get; set; }
        public List<string>         GivenArguments { get; set; }
        public List<Command>        SubCommands { get; set; }
        public Command              ParentCommand { get; set; }
        public bool                 ExecutionAllowed { get; set; }
        public string               CommandReverse { get; set; }
        public int                  CommandDepth { get; set; }

        public Command RootCommand
        {
            get
            {
                Command rootCommand = ParentCommand;

                while(rootCommand != null)
                {
                    if (rootCommand.ParentCommand != null)
                        rootCommand = rootCommand.ParentCommand;
                    else
                        break;
                }

                System.Diagnostics.Debug.WriteLine("Command: " + this.CommandName + "Command Root: " + rootCommand);
                return rootCommand;
            }
        }

        public Command()
        {
            SubCommands = new List<Command>();
            ExecutionAllowed = true;

            InitializeSubCommands();
        }

        public static Command Initialize(Type t, Command parent = null)
        {
            Type type = t;

            CommandAttribute commandAtr = (CommandAttribute)type.GetCustomAttribute(typeof(CommandAttribute));
            ArgumentsAttribute argAtr = (ArgumentsAttribute) type.GetCustomAttribute(typeof(ArgumentsAttribute));

            Command thisCommand = (Command)Activator.CreateInstance(type);

            thisCommand.CommandName = commandAtr.CommandName;
            thisCommand.CommandAlias = commandAtr.CommandAlias;
            thisCommand.HelpMessage = commandAtr.CommandHelpMessage;

            if (type.GetCustomAttribute(typeof(ArgumentsAttribute), true) != null)
            {
                thisCommand.CommandRequiredAttributes = argAtr.RequiredParams;
                thisCommand.CommandOptionalAttributes = argAtr.OptionalParams;
            }

            thisCommand.ParentCommand = parent;

            //TODO: Fix commandreverselookup not working
            thisCommand.CommandReverse = CommandReverseLookup(thisCommand);

            System.Diagnostics.Debug.WriteLine("-------COMMAND BEGIN-------");
            System.Diagnostics.Debug.WriteLine("this command name: " + commandAtr.CommandName);
            System.Diagnostics.Debug.WriteLine("this command Parent: " + commandAtr.ParentCommand);
            System.Diagnostics.Debug.WriteLine("COMMAND REVERSE: " + thisCommand.CommandReverse);
            System.Diagnostics.Debug.WriteLine("-------COMMAND End---------");

            return thisCommand;
        }

        /// <summary>
        /// Looks through class files seatching for command and argument atrributes
        /// then adds subcommands to this command through its defined parent command
        /// </summary>
        public void InitializeSubCommands()
        {
            foreach(Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if(type.GetCustomAttributes(typeof(CommandAttribute), true).Length > 0)
                {
                    CommandAttribute commandAtr = (CommandAttribute) type.GetCustomAttribute(typeof(CommandAttribute));

                    if (commandAtr.ParentCommand != null && commandAtr.ParentCommand.Equals(this.GetType()))
                    {
                        Command subC = Initialize(type, this);
                        SubCommands.Add(subC);
                    }
                }
            }
        }

        public string GetHelpMessage(Command bc)
        {
            string returnString = "";

            returnString += CommandReverseLookup(bc) + " \t \t \t";
            returnString += bc.HelpMessage + System.Environment.NewLine;

            return returnString;
        }

        public string GetAllHelpMessages()
        {
            string allHelp = "";

            foreach (Command bc in SubCommands)
            {
                allHelp += GetHelpMessage(bc);
            }

            return allHelp;
        }

        public static string CommandReverseLookup(Command command)
        {
            string commandRootPathString = "";
            List<string> commandRootPath = new List<string>
            {
                command.CommandName
            };

            Command currentLocation = command;

            while(currentLocation.ParentCommand != null)
            {
                currentLocation = currentLocation.ParentCommand;

                if (currentLocation != null)
                    commandRootPath.Add(currentLocation.CommandName);
            }

            commandRootPath.Reverse();

            foreach (string com in commandRootPath)
            {
                commandRootPathString += com + " ";
            }

            return commandRootPathString;
        }

        public string GetSpecificHelpMessage()
        {
            string[] requiredParams = this.CommandRequiredAttributes;
            string[] optionalParams = this.CommandOptionalAttributes;


            string helpMessageT = System.Environment.NewLine;
            helpMessageT += GetHelpMessage(this);

            helpMessageT += "Use like: " + CommandReverseLookup(this);

            if(requiredParams != null)
            {
                for (int i = 0; i < requiredParams.Length; i++)
                {
                    helpMessageT += " <" + requiredParams[i] + ">";
                }
            }
            if (optionalParams != null)
            {
                helpMessageT += ", optional [";

                for (int i = 0; i < optionalParams.Length; i++)
                {
                    helpMessageT += " <" + optionalParams[i] + ">";
                }

                helpMessageT += "]";
            }

            return helpMessageT += System.Environment.NewLine;
        }

        public bool ParametersEmpty()
        {
            if (GivenArguments.Count == 0)
                return true;
            else
                return false;
        }

        public virtual string Run(List<string> arguments)
        {
            GivenArguments = arguments;

            if(!ParametersEmpty())
            {
                //return InitParameters();

                if(arguments[0] == "help" || arguments[0] == "hlp")
                {
                    if (SubCommands.Count != 0)
                        return GetAllHelpMessages();
                    else
                        return GetSpecificHelpMessage();
                }
                else
                {
                    foreach (Command subCommand in SubCommands)
                    { 
                        if (subCommand.CommandName == GivenArguments[0] || subCommand.CommandAlias == GivenArguments[0])
                        {
                            arguments.RemoveAt(0);
                            return subCommand.Run(arguments);
                        }
                    }

                    if(CommandRequiredAttributes != null)
                        return Execute();

                    string argumentList = "";

                    foreach (string arg in arguments)
                    {
                        argumentList += arg + " ";
                    }

                    return "\"" + CommandReverse + " " + argumentList + "\"" + "is not available, type: " + CommandName + "/" + CommandAlias + " help" + " for a list of all arguments." + Environment.NewLine;
                }
            }
            else
            {
                if(CommandRequiredAttributes == null)
                    return Execute();
                else
                    return GlobalMessages.TOO_FEW_ARGUMENTS + " Use " + CommandReverse + " help, for a list of all suitable arguments" + Environment.NewLine;
            }
        }


        public virtual string Execute()
        {
            if (RootCommand != null)
                return RootCommand.RootExecution();
            else
                return "Not implemented yet";
        }

        public virtual string RootExecution()
        {
            return "";
        }
    }
}
