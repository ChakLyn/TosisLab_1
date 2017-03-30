//-----------------------------------------------------------------------
// <copyright file="Command.cs" company="My Company">
//     Some info
// </copyright>
//-----------------------------------------------------------------------

namespace TosisLab_1.Infrastructure
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Class which implements ICommand 
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Action delegate which will execute
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// Predicate that which check delegate can be executed
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">what will be executed</param>
        public Command(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="execute">what will be executed</param>
        /// <param name="canExecute">can be executed</param>
        public Command(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Event handler on case if executable ability has been changed  
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Check can action can be executed
        /// </summary>
        /// <param name="parameter">come parameter for predicate</param>
        /// <returns>can execute?</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute.Invoke(parameter);
        }

        /// <summary>
        /// Invoke command
        /// </summary>
        /// <param name="parameter">some parameter for command</param>
        public void Execute(object parameter)
        {
            this.execute.Invoke(parameter);
        }
    }
}
