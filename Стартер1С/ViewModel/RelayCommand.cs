using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Стартер1С.ViewModel
{

//    public class RelayCommand<T> : ICommand
//    {
//        private Action<T> action;
//        public RelayCommand(Action<T> action) => this.action = action;
//        public bool CanExecute(object parameter) => true;
//#pragma warning disable CS0067
//        public event EventHandler CanExecuteChanged;
//#pragma warning restore CS0067
//        public void Execute(object parameter) => action((T)parameter);
//    }

//    public class RelayCommand : ICommand
//    {
//        private Action action;
//        public RelayCommand(Action action) => this.action = action;
//        public bool CanExecute(object parameter) => true;
//#pragma warning disable CS0067
//        public event EventHandler CanExecuteChanged;
//#pragma warning restore CS0067
//        public void Execute(object parameter) => action();
//    }


    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }

}
