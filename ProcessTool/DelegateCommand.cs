using System;
using System.Runtime.Remoting.Activation;
using System.Windows.Input;

namespace ProcessTool
{
    /// <summary>
    /// Delegatecommand
    /// </summary>
    public class DelegateCommand : ICommand
    {
        Func<object, bool> canExecute;
        Action<object> executeAction;
        bool canExecuteCache;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute = null)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;
            bool temp = canExecute(parameter);

            if (canExecuteCache != temp)
            {
                canExecuteCache = temp;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }

            return canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }

        #endregion
    }

    public class DelegateCommand<T> : ICommand
    {
        Func<T, bool> canExecute;
        Action<T> executeAction;
        bool canExecuteCache;

        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecute = null)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
                return true;
            bool temp = canExecute((T)parameter);

            if (canExecuteCache != temp)
            {
                canExecuteCache = temp;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }

            return canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            executeAction((T)parameter);
        }

        #endregion
    }
}
