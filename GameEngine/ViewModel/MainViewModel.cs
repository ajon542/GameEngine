using System;
using System.Collections.Generic;
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
        public DockManagerViewModel DockManagerViewModel { get; private set; }

        public MenuViewModel MenuViewModel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Create the view-models.
            var documents = new List<DockWindowViewModel>();
            documents.Add(new SceneViewModel() { Title = "Scene " });
            documents.Add(new GameViewModel() { Title = "Game " });

            this.DockManagerViewModel = new DockManagerViewModel(documents);
            this.MenuViewModel = new MenuViewModel(documents);

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
            logger.Log(LogLevel.Fatal, e.ExceptionObject as Exception, e.ExceptionObject.ToString());
        }
    }
}
