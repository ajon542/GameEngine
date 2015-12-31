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

        #region KeyDown Command

        /// <summary>
        /// Gets or sets the KeyDown Command Property.
        /// </summary>
        public new ICommand KeyDown
        {
            get { return (ICommand)GetValue(KeyDownProperty); }
            set { SetValue(KeyDownProperty, value); }
        }

        /// <summary>
        /// The key down dependency property.
        /// </summary>
        /// <remarks>
        /// KeyDown="{Binding KeyDownCommand}"
        /// </remarks>
        public static readonly DependencyProperty KeyDownProperty =
            DependencyProperty.Register("KeyDown", typeof(ICommand), typeof(OpenGLControl),
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

        #region Event Handlers

        /// <summary>
        /// Control loaded event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (SceneInitialized == null)
            {
                return;
            }
            SceneInitialized.Execute(new GraphicsProperties(glControl.Width, glControl.Height));
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
            glControl.KeyDown += OnKeyDown;
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
        /// On key down event handler.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (KeyDown != null)
            {
                KeyDown.Execute(e);
            }
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
                Update.Execute(null);
            }
            
            initialUpdate = true;

            // Force the GL control to paint.
            glControl.Invalidate();
        }

        #endregion
    }
}
