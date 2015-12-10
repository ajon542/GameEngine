using System.Windows;
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
    }
}
