using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.View
{
    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    public partial class SceneView : System.Windows.Controls.UserControl
    {
        GLControl glControl;

        public SceneView()
        {
            InitializeComponent();
        }

        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            var flags = GraphicsContextFlags.Default;

            glControl = new GLControl(new GraphicsMode(32, 24), 2, 0, flags);
            glControl.MakeCurrent();
            glControl.Paint += Paint;
            glControl.Dock = DockStyle.Fill;
            (sender as WindowsFormsHost).Child = glControl;
            SetupViewport();
        }

        private void SetupViewport()
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            double aspectRatio = glControl.Width / (double)glControl.Height;
            float fov = 1.0f;
            float near = 1.0f;
            float far = 1000.0f;

            Matrix4 perspectiveMatrix =
               Matrix4.CreatePerspectiveFieldOfView(fov, (float)aspectRatio, near, far);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMatrix);
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            GL.ClearColor(
                (float)Red.Value,
                (float)Green.Value,
                (float)Blue.Value,
                1);

            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit |
                ClearBufferMask.StencilBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0, 0, -5);
            GL.Color3(Color.Yellow);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(1, 1);
            GL.Vertex2(-1, 1);
            GL.Vertex2(-1, -1);
            GL.Vertex2(1, -1);
            GL.End();

            glControl.SwapBuffers();
        }

        private void Resized(object sender, RoutedEventArgs e)
        {
            SetupViewport();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            glControl.Invalidate();
        }
    }
}
