using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Gestionix.POS.GUI.ViewModels
{
    public abstract class VMBase : INotifyPropertyChanged
    {
        #region Fields
        protected VMBase _mainview;
        protected ObservableCollection<string> _messages;
        protected bool _aremessagesvisible;
        #endregion

        #region Properties
        public VMBase MainView
        {
            get { return _mainview; }
            set
            {
                if (value != _mainview)
                {
                    _mainview = value;
                    RaisePropertyChanged("MainView");
                }
            }
        }

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
        }

        public bool AreMessagesVisible
        {
            get { return _aremessagesvisible; }
            set
            {
                if (_aremessagesvisible != value)
                {
                    _aremessagesvisible = value;
                    RaisePropertyChanged("AreMessagesVisible");

                    if (!_aremessagesvisible) _messages.Clear();
                }
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Helpers
        protected void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaiseEvent(object sender, EventHandler e)
        {
            if (e != null)
                e(sender, new EventArgs());
        }
        #endregion
    }
}
