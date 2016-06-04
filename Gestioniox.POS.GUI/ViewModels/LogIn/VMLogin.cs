using Gestionix.POS.Core.Controllers;
using System;
using System.Windows.Input;

namespace Gestionix.POS.GUI.ViewModels.Login
{
    public class VMLogin : VMBase
    {
        #region EventHandlers
        public event EventHandler OnLogIn;
        #endregion

        #region Fields
        private PCAccess _accesscontroller;
        private ICommand _logingcommand;
        private bool _islogged;
        private bool _islogging;
        #endregion

        #region Properties
        public string TxtUsername { get; set; }
        public string TxtPassword { get; set; }

        public bool IsLogged
        {
            get { return _islogged; }
            private set
            {
                if (_islogged != value)
                {
                    _islogged = value;
                    RaiseEvent(this, OnLogIn);
                }
            }
        }

        public bool IsLogging
        {
            get { return _islogging; }
            set
            {
                if(_islogging != value)
                {
                    _islogging = value;
                    RaisePropertyChanged("IsLogging");
                }
            }
        }
        #endregion

        #region Commands
        public ICommand LogInCommand { get { return _logingcommand; } }
        #endregion

        #region Ctors
        public VMLogin()
        {
            _islogged = false;
            _accesscontroller = new PCAccess();
            _logingcommand = new GenericCommand(LogInAction);
        }
        #endregion

        #region Actions
        private async void LogInAction()
        {
            try
            {                
                bool IsAuthenticated = await System.Threading.Tasks.Task.Run(() =>  _accesscontroller.Authenticate(TxtUsername, TxtPassword) );                

                if (IsAuthenticated)
                {
                    //TODO:
                    IsLogged = true;
                }
                else
                    IsLogging = false;
            }
            catch (Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }
        }
        #endregion
    }
}
