using Gestionix.POS.GUI.ViewModels.Login;
using System;
using System.Windows.Input;

namespace Gestionix.POS.GUI.ViewModels.POS
{
    public class VMPOS : VMBase
    {
        #region Fields
        private GenericCommand _loginaction;
        private string _title;
        private bool _isresizable;
        private bool _ismaximized;
        #endregion

        #region Properties        
        public string TxtTitle
        {
            get { return _title; }
            private set
            {
                if(_title != value)
                {
                    _title = value;
                    RaisePropertyChanged("TxtTitle");
                }
            }
        }

        public bool IsResizable
        {
            get { return _isresizable; }
            private set
            {
                if(_isresizable != value)
                {
                    _isresizable = value;
                    RaisePropertyChanged("IsResizable");
                }
            }
        }

        public bool IsMaximized
        {
            get { return _ismaximized; }
            private set
            {
                if (_ismaximized != value)
                {
                    _ismaximized = value;
                    RaisePropertyChanged("IsMaximized");
                }
            }
        }
        #endregion

        #region Commands
        public ICommand InCommand
        {
            get { return _loginaction; }
        }
        #endregion

        #region Ctors
        public VMPOS()
        {
            Initialize();
        }
        #endregion

        #region Helpers
        private void LoginAction()
        {
            TxtTitle = "Gestionox Punto de venta";
            MainView = null;
            IsResizable = true;
            IsMaximized = true;
        }
        #endregion

        #region Helpers
        private void Initialize()
        {
            _title = POSResources.S("D_Login");
            _loginaction = new GenericCommand(LoginAction);
            VMLogin LoginView = new VMLogin();
            LoginView.InCommand = _loginaction;
            _mainview = LoginView;
            _isresizable = false;
            _ismaximized = false;
        }
        #endregion
    }
}
