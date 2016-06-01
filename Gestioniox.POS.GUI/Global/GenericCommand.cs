using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gestionix.POS.GUI
{
    public class GenericCommand : ICommand
    {
        private object _parameter, _parameter2;
        private Action _what;
        private Action<object> _what_param;
        private Action<object, object> _what_params;
        private Func<bool> _when;
        public event EventHandler CanExecuteChanged;

        #region Constructors
        public GenericCommand(Action<object> what, object parameter)
        {
            _parameter = parameter;
            _what_param = what;
            _when = DefaulValidation;
        }

        public GenericCommand(Action<object> what)
        {
            _what_param = what;
            _when = DefaulValidation;
        }

        public GenericCommand(Action what)
        {
            _what = what;
            _when = DefaulValidation;
        }

        public GenericCommand(Action what, Func<bool> when)
        {
            _what = what;
            _when = when;
        }
        #endregion

        public bool CanExecute(object parameter)
        {
            return _when();
        }

        public void Execute(object parameter)
        {
            if (_what_param != null)
            {
                _parameter = parameter ?? _parameter;
                _what_param(_parameter);
            }
            else if (_what_params != null)
            {
                _what_params(_parameter, _parameter2);
            }
            else if (_what != null)
                _what();
        }

        private bool DefaulValidation()
        {
            return true;
        }
    }
}
