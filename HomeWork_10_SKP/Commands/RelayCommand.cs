using System;
using System.Windows.Input;

namespace HomeWork_10_SKP
{
    public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;

        readonly Predicate<object> _canExecute;

        #endregion

        #region Methods
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if(execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            { _execute(parameter); }
        }

        #endregion

        #region Events
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value;}
        }
        #endregion

    }
}
