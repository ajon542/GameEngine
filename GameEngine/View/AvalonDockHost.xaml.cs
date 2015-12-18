using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameEngine.View
{
    /// <summary>
    /// Interaction logic for AvalonDockHost.xaml
    /// </summary>
    public partial class AvalonDockHost : UserControl
    {
        public AvalonDockHost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event raised when Avalondock has loaded.
        /// </summary>
        public event EventHandler<EventArgs> AvalonDockLoaded;

        /// <summary>
        /// Event raised when Avalondock has loaded.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void AvalonDock_Loaded(object sender, RoutedEventArgs e)
        {
            EventHandler<EventArgs> handler = AvalonDockLoaded;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
