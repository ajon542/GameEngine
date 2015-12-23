using System;
using System.Windows.Input;

namespace GameEngine.ViewModel
{
    /// <summary>
    /// Implementation of the ICommand interface.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// The action to execute.
        /// </summary>
        private Action<object> execute;

        /// <summary>
        /// The predicate for determining whether to execute the command.
        /// </summary>
        private Predicate<object> canExecute;

        /// <summary>
        /// Internal event handler.
        /// </summary>
        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        /// CanExecuteChanged event handler.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public DelegateCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate for determining whether to execute the command.</param>
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Determine if we can execute the command based on the predicate.
        /// </summary>
        /// <param name="parameter">The parameter to pass.</param>
        /// <returns>True if the commmand can be executed; false otherwise.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute != null && canExecute(parameter);
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter">The parameter to pass.</param>
        public void Execute(object parameter)
        {
            // See: http://stackoverflow.com/questions/3502127/in-wpf-how-do-i-implement-icommandsource-to-give-my-custom-control-ability-to-us
            execute(parameter);
        }

        /// <summary>
        /// Invoke the event handler.
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Execute the command always by default.
        /// </summary>
        /// <param name="parameter">The parameter to pass.</param>
        /// <returns>True.</returns>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
