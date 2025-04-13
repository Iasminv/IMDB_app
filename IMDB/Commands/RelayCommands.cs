using System;
using System.Windows.Input;

namespace IMDB.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executeNoParameter;
        private readonly Func<bool> _canExecuteNoParameter;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeNoParameter = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteNoParameter = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteNoParameter == null || _canExecuteNoParameter();
        }

        public void Execute(object parameter)
        {
            _executeNoParameter();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
