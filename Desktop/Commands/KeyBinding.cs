using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Desktop.Commands
{
    public class KeyBinding : ICommand
    {
        public Key GestureKey { get; set; }
        public ModifierKeys GestureModifier { get; set; }
        public MouseAction MouseGesture { get; set; }

        Action<object> _executeDelegate;

        public KeyBinding(Action<object> executeDelegate)
        {
            _executeDelegate = executeDelegate;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeDelegate(parameter);
        }
    }
}
