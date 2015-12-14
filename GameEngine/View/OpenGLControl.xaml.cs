using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
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
        /// <summary>
        /// Reference to the OpenGL control.
        /// </summary>
        private GLControl glControl;

        /// <summary>
        /// Used for frame rate calculation.
        /// </summary>
        private DateTime lastMeasureTime;

        #region DependencyProperty Examples
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
        #endregion

        #region ICommand Examples
        // This is an example of command execution. We can hook a command up in the xaml and have
        // it execute in the view-model. Command="{Binding InitializedCommand}"
        public ICommand Command
        {
            get 
            {
                return (ICommand)GetValue(CommandProperty); 
            }
            set
            {
                SetValue(CommandProperty, value); 
            }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(OpenGLControl), new UIPropertyMetadata(null));
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLControl"/> class.
        /// </summary>
        public OpenGLControl()
        {
            InitializeComponent();

            // In design view, there is an error due to memory access permissions.
            // Prevent any GL calls during this time.
            if(DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            SetupTimer();

            Loaded += OnLoaded;
        }

        /// <summary>
        /// Set the timer for invalidating the GLControl.
        /// </summary>
        private void SetupTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += OnTimerOnTick;
            timer.Start();

            Core.Timer.Instance.Init();
        }

        /// <summary>
        /// Set the viewport.
        /// </summary>
        private void SetupViewport()
        {
            // Set the view port.
            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            // Create the perspective field of view matrix.
            double aspectRatio = glControl.Width / (double)glControl.Height;
            float fov = 1.0f;
            float near = 1.0f;
            float far = 1000.0f;

            Matrix4 perspectiveMatrix =
               Matrix4.CreatePerspectiveFieldOfView(fov, (float)aspectRatio, near, far);

            // Set the matrix mode and load the matrix.
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMatrix);
        }

        #region Event Handlers

        /// <summary>
        /// Control loaded event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Windows forms host initialized event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnHostInitialized(object sender, EventArgs e)
        {
            // In design view, there is an error due to memory access permissions.
            // Prevent any GL calls during this time.
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // Initialize the GL control.
            var flags = GraphicsContextFlags.Default;

            glControl = new GLControl(new GraphicsMode(32, 24), 2, 0, flags);
            glControl.MakeCurrent();
            glControl.Paint += OnPaint;
            glControl.Dock = DockStyle.Fill;
            //glControl.VSync = false; This call only gains about 5 fps, wonder what happens on my work machine?
            (sender as WindowsFormsHost).Child = glControl;

            // Set the view port.
            SetupViewport();
        }

        /// <summary>
        /// Paint event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            // Update the timer instance.
            Core.Timer.Instance.Update();

            // Clear.
            GL.ClearColor(Color.Black);
            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit |
                ClearBufferMask.StencilBufferBit);

            // Render the scene list.
            foreach (Scene scene in SceneList)
            {
                scene.Render();
            }

            // Swap the buffers.
            glControl.SwapBuffers();
        }

        /// <summary>
        /// Resized event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnResized(object sender, RoutedEventArgs e)
        {
            SetupViewport();
        }

        /// <summary>
        /// Timer event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnTimerOnTick(object sender, EventArgs e)
        {
            // Update the fps counter every second.
            if (DateTime.Now.Subtract(lastMeasureTime) > TimeSpan.FromSeconds(1))
            {
                FpsCounter.Content = Core.Timer.Instance.Fps;
                lastMeasureTime = DateTime.Now;
            }

            // Force the GL control to paint.
            glControl.Invalidate();
        }

        #endregion
    }
}
