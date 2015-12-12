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

using GameEngine.Core;

namespace GameEngine.View
{
    /// <summary>
    /// Interaction logic for OpenGLControl.xaml
    /// </summary>
    public partial class OpenGLControl : System.Windows.Controls.UserControl
    {
        private GLControl glControl;

        private DateTime lastMeasureTime;

        // TODO: Determine if there is a better way to do this.
        // This basically allows the SceneViewModel to add scenes to the scenelist and have them rendered.
        // I am going to go ahead with this method and see what issues I run into.
        public static readonly DependencyProperty SceneListProperty =
            DependencyProperty.Register("SceneList", typeof(List<Scene>), typeof(OpenGLControl),
            new PropertyMetadata(new List<Scene>(), new PropertyChangedCallback(OnSceneListUpdated)));

        private static void OnSceneListUpdated(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            // This is an example of dependency property changed notification.
        }

        public List<Scene> SceneList
        {
            get { return (List<Scene>)GetValue(SceneListProperty); }
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

        bool init = true;

        private void Paint(object sender, PaintEventArgs e)
        {
            if(init)
            {
                // TODO: Remove this hack.
                // The scene has to be initialized after the WindowsFormsHost_Initialized call.
                // The problem is at the time of the call to WindowsFormsHost_Initialized, we
                // have not received the active SceneList. We need to put in some framework to
                // to notify the rest of the application that OpenGL has been initialized.
                foreach (Scene scene in SceneList)
                {
                    scene.Initialize();
                }
                init = false;
            }

            // Update the timer instance.
            Core.Timer.Instance.Update();

            GL.ClearColor(Color.Red);
            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit |
                ClearBufferMask.StencilBufferBit);

            // Render the scene list.
            foreach (Scene scene in SceneList)
            {
                scene.Render();
            }

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
