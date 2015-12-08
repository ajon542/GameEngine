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

namespace GameEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GLControl glControl;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            var flags = GraphicsContextFlags.Default;

            glControl = new GLControl(new GraphicsMode(32, 24), 2, 0, flags);
            glControl.MakeCurrent();
            glControl.Paint += GLControl_Paint;
            glControl.Dock = DockStyle.Fill;
            (sender as WindowsFormsHost).Child = glControl;
            SetupViewport();
        }

        private void SetupViewport()
        {
            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            double aspect_ratio = glControl.Width / (double)glControl.Height;
            float fov = 1.0f;
            float near_distance = 1.0f;
            float far_distance = 1000.0f;

            OpenTK.Matrix4 perspective_matrix =
               OpenTK.Matrix4.CreatePerspectiveFieldOfView(fov, (float)aspect_ratio, near_distance, far_distance);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective_matrix);
        }

        private void GLControl_Paint(object sender, PaintEventArgs e)
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

            GL.Begin(BeginMode.Quads);
            GL.Vertex2(1, 1);
            GL.Vertex2(-1, 1);
            GL.Vertex2(-1, -1);
            GL.Vertex2(1, -1);
            GL.End();

            glControl.SwapBuffers();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            glControl.Invalidate();
        }
    }
}
