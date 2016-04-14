using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Threading;
using GameEngine.Core.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using NLog;

namespace GameEngine.View
{
    /// <summary>
    /// Interaction logic for OpenGLControl.xaml
    /// </summary>
    public partial class OpenGLControl : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// Reference to the logging mechanism.
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Reference to the OpenGL control.
        /// </summary>
        private GLControl glControl;

        /// <summary>
        /// Used for frame rate calculation.
        /// </summary>
        private DateTime lastMeasureTime;

        /// <summary>
        /// Whether the first call to update has occurred.
        /// </summary>
        /// <remarks>
        /// The update mechanism is called on a timer event. If this event does not
        /// fire before the first call to Render, bad things can happen. Ensure the
        /// the update occurs first.
        /// </remarks>
        private bool initialUpdate;

        #region Initialized Command

        /// <summary>
        /// Gets or sets the SceneInitialized Command Property.
        /// </summary>
        public ICommand SceneInitialized
        {
            get { return (ICommand)GetValue(SceneInitializedProperty); }
            set { SetValue(SceneInitializedProperty, value); }
        }

        /// <summary>
        /// The initialized dependency property.
        /// </summary>
        /// <remarks>
        /// SceneInitialized="{Binding SceneInitializedCommand}"
        /// </remarks>
        public static readonly DependencyProperty SceneInitializedProperty =
            DependencyProperty.Register("SceneInitialized", typeof(ICommand), typeof(OpenGLControl),
                new UIPropertyMetadata(null));

        #endregion

        #region Update Command

        /// <summary>
        /// Gets or sets the Update Command Property.
        /// </summary>
        public ICommand Update
        {
            get { return (ICommand)GetValue(UpdateProperty); }
            set { SetValue(UpdateProperty, value); }
        }

        /// <summary>
        /// The update dependency property.
        /// </summary>
        /// <remarks>
        /// Update="{Binding UpdateCommand}"
        /// </remarks>
        public static readonly DependencyProperty UpdateProperty =
            DependencyProperty.Register("Update", typeof (ICommand), typeof (OpenGLControl),
                new UIPropertyMetadata(null));

        #endregion

        #region Render Command

        /// <summary>
        /// Gets or sets the Render Command Property.
        /// </summary>
        public ICommand Render
        {
            get { return (ICommand)GetValue(RenderProperty); }
            set { SetValue(RenderProperty, value); }
        }

        /// <summary>
        /// The render dependency property.
        /// </summary>
        /// <remarks>
        /// Render="{Binding RenderCommand}"
        /// </remarks>
        public static readonly DependencyProperty RenderProperty =
            DependencyProperty.Register("Render", typeof(ICommand), typeof(OpenGLControl),
                new UIPropertyMetadata(null));

        #endregion

        #region Resized Command

        /// <summary>
        /// Gets or sets the Resized Command Property.
        /// </summary>
        public ICommand Resized
        {
            get { return (ICommand)GetValue(ResizedProperty); }
            set { SetValue(ResizedProperty, value); }
        }

        /// <summary>
        /// The resized dependency property.
        /// </summary>
        /// <remarks>
        /// Resized="{Binding ResizedCommand}"
        /// </remarks>
        public static readonly DependencyProperty ResizedProperty =
            DependencyProperty.Register("Resized", typeof(ICommand), typeof(OpenGLControl),
                new UIPropertyMetadata(null));

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLControl"/> class.
        /// </summary>
        public OpenGLControl()
        {
            logger.Log(LogLevel.Info, "Creating open gl control view");

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
            logger.Log(LogLevel.Info, "");

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += OnTimerOnTick;
            timer.Start();

            Core.Timer.Instance.Init();
        }

        #region Event Handlers

        /// <summary>
        /// Control loaded event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            logger.Log(LogLevel.Info, "");

            if (SceneInitialized != null)
            {
                SceneInitialized.Execute(new GraphicsProperties(glControl.Width, glControl.Height));
            }
        }

        /// <summary>
        /// Windows forms host initialized event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnHostInitialized(object sender, EventArgs e)
        {
            logger.Log(LogLevel.Info, "");

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
        }

        /// <summary>
        /// Paint event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (initialUpdate == false)
            {
                return;
            }

            // Update the timer instance.
            Core.Timer.Instance.Update();

            // Clear.
            GL.ClearColor(Color.Black);
            GL.Clear(
                ClearBufferMask.ColorBufferBit |
                ClearBufferMask.DepthBufferBit |
                ClearBufferMask.StencilBufferBit);

            // Execute the render command.
            if (Render != null)
            {
                Render.Execute(null);
            }

            // Swap the buffers.
            GL.Flush();
            glControl.SwapBuffers();
        }

        /// <summary>
        /// Resized event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnResized(object sender, RoutedEventArgs e)
        {
            if(Resized != null)
            {
                Resized.Execute(new GraphicsProperties(glControl.Width, glControl.Height));
            }
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

            // Execute the update command.
            if (Update != null)
            {
                // TODO: This occurs even when the window is not focused. Should it?
                Update.Execute(null);
            }
            
            initialUpdate = true;

            // Force the GL control to paint.
            glControl.Invalidate();
        }

        #endregion
    }
}
