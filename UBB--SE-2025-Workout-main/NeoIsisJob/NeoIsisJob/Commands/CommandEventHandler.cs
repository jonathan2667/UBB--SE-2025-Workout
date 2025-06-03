using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NeoIsisJob.Commands
{
    public class CommandEventHandler<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<T> Action;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.Action((T)parameter);
        }
        public CommandEventHandler(Action<T> action)
        {
            this.Action = action;
        }
    }
}