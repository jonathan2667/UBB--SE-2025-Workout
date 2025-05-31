using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace NeoIsisJob.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) 
        {
            if (canExecute == null) return true;
            
            var convertedParameter = ConvertParameter(parameter);
            return canExecute(convertedParameter);
        }

        public void Execute(object parameter) 
        {
            var convertedParameter = ConvertParameter(parameter);
            execute(convertedParameter);
        }

        private T ConvertParameter(object parameter)
        {
            if (parameter == null)
                return default(T);

            // If parameter is already the correct type, return it
            if (parameter is T)
                return (T)parameter;

            // Try to convert the parameter to type T
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null && converter.CanConvertFrom(parameter.GetType()))
                {
                    return (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, parameter);
                }

                // Special handling for common conversions
                if (typeof(T) == typeof(int) && parameter is string stringParam)
                {
                    if (int.TryParse(stringParam, out int intValue))
                        return (T)(object)intValue;
                }

                if (typeof(T) == typeof(double) && parameter is string stringParamDouble)
                {
                    if (double.TryParse(stringParamDouble, out double doubleValue))
                        return (T)(object)doubleValue;
                }

                // Fallback: try direct conversion
                return (T)Convert.ChangeType(parameter, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                // If conversion fails, return default value
                return default(T);
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    // Non-generic RelayCommand for parameterless actions
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute();
        public void Execute(object parameter) => execute();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}