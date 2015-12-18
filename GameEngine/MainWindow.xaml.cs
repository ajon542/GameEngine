using System.Windows;
using System.Windows.Input;
using GameEngine.ViewModel;

namespace GameEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Example of switching between views based on the view-model.
            DataContext = new MainViewModel();
        }

        /// <summary>
        /// Exit the application.
        /// </summary>
        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ApplicationShutdown();
        }

        private void ApplicationShutdown()
        {
            Application.Current.Shutdown();
        }
    }
}
