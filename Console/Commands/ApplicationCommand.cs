using System;
using System.IO;
using Bugtracker.InternalApplication;

namespace Bugtracker.Console
{
    class ApplicationCommand : ICommand
    {
        private readonly Application _application;
        private readonly ApplicationAction _applicationAction;
        private readonly string _applicationName;
        private readonly string _applicationExecutablePath;
        private readonly bool _applicationStandard;
        private readonly bool _applicationShowSpec;

        private readonly string _tempParameter;

        public ApplicationCommand(Application app, ApplicationAction appAction, string tempParameter)
        {
            _application = app;
            _applicationAction = appAction;
            _tempParameter = tempParameter;
        }

        
        private bool isParameterValid(ApplicationAction appAction, string tempParameter)
        {
            switch(_applicationAction)
            {
                case ApplicationAction.name:
                    if (tempParameter.Length != 0)
                        return true;
                    else
                        throw new ParameterNotValidExeption(tempParameter, tempParameter, appAction.ToString());
                case ApplicationAction.exe:
                    if (tempParameter.Length != 0)
                    {
                        //check if path is realy an existing path on this machine -> throw exception if not
                        Path.GetFullPath(tempParameter);
                        return true;
                    }

                    else
                        throw new NotImplementedException();
                case ApplicationAction.showspec:
                        
                    return false;

                case ApplicationAction.standard:

                        return false;

                default:
                        return false;
            }
        }

        public string ExecuteAction()
        {
            switch(_applicationAction)
            {
                case ApplicationAction.name:

                    throw new NotImplementedException();

                    break;
                case ApplicationAction.exe:

                    throw new NotImplementedException();

                    break;
                case ApplicationAction.showspec:

                    throw new NotImplementedException();

                    break;
                case ApplicationAction.standard:

                    throw new NotImplementedException();
                default:
                    throw new CommandActionDoesNotExistEception();
                    break;
                
            }
        }
    }
}
