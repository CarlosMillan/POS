using Gestionix.POS.GUI.ViewModels.Login;
using System;
using System.Windows.Input;

namespace Gestionix.POS.GUI.ViewModels.POS
{
    public class VMPOS : VMBase
    {
        #region Fields        
        #region Views
        private VMLogin _loginview;
        #endregion
        private string _title;
        private bool _isresizable;
        private bool _ismaximized;
        private bool _islogged;
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
        #endregion

        #region Ctors
        public VMPOS()
        {
            Initialize();
        }
        #endregion

        #region Actions
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
            _loginview = new VMLogin();
            _loginview.OnLogIn += _loginview_OnLogIn;
            _mainview = _loginview;
            _isresizable = false;
            _ismaximized = false;
        }

        private void _loginview_OnLogIn(object sender, EventArgs e)
        {
            try
            {
                LoginAction();
            }
            catch(Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }
        }
        #endregion
    }
}
