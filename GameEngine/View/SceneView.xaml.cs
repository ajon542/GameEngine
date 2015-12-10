using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Threading;

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
        private int frames;
        private int rotation;

        private GLControl glControl;

        private DateTime lastMeasureTime;

        public SceneView()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += TimerOnTick;
            timer.Start();
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
            frames++;

            rotation++;
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
            GL.Rotate(rotation, new Vector3d(0, 0, 1));
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

        private void TimerOnTick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(this.lastMeasureTime) > TimeSpan.FromSeconds(1))
            {
                FpsCounter.Content = frames.ToString();
                frames = 0;
                lastMeasureTime = DateTime.Now;
            }
            glControl.Invalidate();
        }
    }
}
