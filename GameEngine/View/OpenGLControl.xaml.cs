using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for OpenGLControl.xaml
    /// </summary>
    public partial class OpenGLControl : System.Windows.Controls.UserControl
    {
        int vbo;

        private GLControl glControl;

        private DateTime lastMeasureTime;

        public static readonly DependencyProperty BlueTextProperty =
        DependencyProperty.Register("BlueText", typeof(string), typeof(OpenGLControl),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBlueTextChanged)));

        private static void OnBlueTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            //OpenGLControl me = o as OpenGLControl;
            // This is an example of dependency property changed notification.
        }

        public string BlueText
        {
            get { return (string)GetValue(BlueTextProperty); }
            set { SetValue(BlueTextProperty, value); }
        }

        // TODO: Determine if there is a better way to do this.
        // This basically allows the SceneViewModel to add scenes to the scenelist and have them rendered.
        // I am going to go ahead with this method and see what issues I run into.
        public static readonly DependencyProperty SceneListProperty =
            DependencyProperty.Register("SceneList", typeof(List<int>), typeof(OpenGLControl),
            new PropertyMetadata(new List<int>(), null));

        public List<int> SceneList
        {
            get { return (List<int>)GetValue(SceneListProperty); }
            set { SetValue(SceneListProperty, value); }
        }

        public OpenGLControl()
        {
            InitializeComponent();

            // In design view, there is an error due to memory access permissions.
            // Prevent any GL calls during this time.
            if(DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // Start a timer for every millisecond.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += TimerOnTick;
            timer.Start();

            Core.Timer.Instance.Init();
        }

        private void CreateVertexBuffer()
        {
            Vector3[] vertices = new Vector3[3];

            for (int i = 0; i < 3; i++)
            {
                float xpos = i / 5f;
                vertices[i] = new Vector3(xpos, 0, -1);
            }

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
                                   new IntPtr(vertices.Length * Vector3.SizeInBytes),
                                   vertices, BufferUsageHint.StaticDraw);
        }

        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            var flags = GraphicsContextFlags.Default;

            glControl = new GLControl(new GraphicsMode(32, 24), 2, 0, flags);
            glControl.MakeCurrent();
            glControl.Paint += Paint;
            glControl.Dock = DockStyle.Fill;
            (sender as WindowsFormsHost).Child = glControl;
            SetupViewport();

            CreateVertexBuffer();
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
            // Render the imaginary scenelist...
            foreach (int i in SceneList)
            {
            }

            // Update the timer instance.
            Core.Timer.Instance.Update();

            GL.ClearColor(Color.Red);
            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit |
                ClearBufferMask.StencilBufferBit);

            GL.Color3(Color.Yellow);
            GL.PointSize(5);
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Points, 0, 3);
            GL.DisableVertexAttribArray(0);

            glControl.SwapBuffers();
        }

        private void Resized(object sender, RoutedEventArgs e)
        {
            SetupViewport();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(lastMeasureTime) > TimeSpan.FromSeconds(1))
            {
                FpsCounter.Content = Core.Timer.Instance.Fps;
                lastMeasureTime = DateTime.Now;
            }
            glControl.Invalidate();
        }
    }
}
