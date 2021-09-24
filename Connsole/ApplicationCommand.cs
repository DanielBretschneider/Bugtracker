using Bugtracker;
using Bugtracker.Connsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                case ApplicationAction.ChangeName:
                    if (tempParameter.Length != 0)
                        return true;
                    else
                        throw new ParameterNotValidExeption(tempParameter, tempParameter, appAction.ToString());
                case ApplicationAction.ChangeExe:
                    if (tempParameter.Length != 0)
                    {
                        //check if path is realy an existing path on this machine -> throw exception if not
                        Path.GetFullPath(tempParameter);
                        return true;
                    }

                    else
                        throw new NotImplementedException();
                        return false;
                    break;
                case ApplicationAction.ChangeShowSpec:
                        return false;

                    break;
                case ApplicationAction.ChangeStandard:

                        return false;
                    break;

                default:
                        return false;
                    break;
            }
        }

        public void ExecuteAction()
        {
            switch(_applicationAction)
            {
                case ApplicationAction.ChangeName:

                    throw new NotImplementedException();

                    break;
                case ApplicationAction.ChangeExe:

                    throw new NotImplementedException();

                    break;
                case ApplicationAction.ChangeShowSpec:

                    throw new NotImplementedException();

                    break;
                case ApplicationAction.ChangeStandard:

                    throw new NotImplementedException();

                    break;

                default:

                    break;
                
            }
        }
    }
}
