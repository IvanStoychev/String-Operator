using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Frontend
{
    class RelayCommand : ICommand
    {
        Action executeMethod;
        Func<bool> canExecuteMethod;
        public event EventHandler CanExecuteChanged = delegate { };

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            if (executeMethod is null) throw new ArgumentNullException("executeMethod");
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public RelayCommand(Action executeMethod) : this(executeMethod, null) { }

        public void Execute(object _)
        {
            if (executeMethod is null) throw new ArgumentNullException("Execute command", "The execution command associated with this RelayCommand is null.");
            executeMethod();
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteMethod is null || canExecuteMethod();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
