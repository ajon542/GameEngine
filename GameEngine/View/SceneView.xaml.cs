using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Threading;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using NLog;

namespace GameEngine.View
{
    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    public partial class SceneView : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// Reference to the logging mechanism.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SceneView()
        {
            logger.Log(LogLevel.Info, "Creating scene view");

            InitializeComponent();
        }
    }
}
