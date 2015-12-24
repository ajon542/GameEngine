using System;
using System.Collections.ObjectModel;
using NLog;

namespace GameEngine.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger instance.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the collection of view-models to display in the view.
        /// </summary>
        public ObservableCollection<ViewModelBase> Settings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Create the view-models.
            Settings = new ObservableCollection<ViewModelBase>
            {
                new SceneViewModel(),
                new GameViewModel()
            };

            // Setup method to handle the unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        /// <summary>
        /// Log any unhandled exceptions.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.LogException(LogLevel.Fatal, e.ExceptionObject.ToString(), e.ExceptionObject as Exception);
        }
    }
}
