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
using System.Windows.Shapes;

using System.Diagnostics;
using System.Windows.Navigation;

namespace GameEngine
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the URL in the default browser.
        /// </summary>
        /// <remarks>
        /// The help window is not going to be very complex so this was
        /// added to the code behind for simplicity. There is no need to
        /// have an MVVM setup here.
        /// </remarks>
        /// <param name="sender">The sender i.e. the Helpwindow.xaml.</param>
        /// <param name="e">The request navigate arguments.</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
