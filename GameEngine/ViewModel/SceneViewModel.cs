using System.Collections.Generic;
using System.Windows.Input;

using GameEngine.Core;
using GameEngine.Core.Graphics;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace GameEngine.ViewModel
{
    /// <summary>
    /// ViewModel for the Scene.
    /// </summary>
    /// <remarks>
    /// Ideally, the ViewModel should have a description of the scene.
    /// Scripts should be attached to the game objects in order to update.
    /// The View should then render the scene based on the description.
    /// </remarks>
    public class SceneViewModel : ViewModelBase
    {
        /// <summary>
        /// List of scenes.
        /// </summary>
        private List<Scene> sceneList;

        /// <summary>
        /// Gets or sets the list of scenes.
        /// </summary>
        public List<Scene> SceneList
        {
            get
            {
                return sceneList;
            }
            set
            {
                sceneList = value;
                OnPropertyChanged("SceneList");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneViewModel"/> class.
        /// </summary>
        public SceneViewModel()
        {
            // TODO: We should really only have a single scene at a time.
            SceneList = new List<Scene> { new Core.GameSpecific.VaoExample() };
        }

        #region Initialized Command

        /// <summary>
        /// Keeps track of whether the view has been loaded previously.
        /// </summary>
        /// <remarks>
        /// Each time we switch from GameView to SceneView, the SceneView is loaded again.
        /// We don't want to be initializing each scene every time. Only do this once.
        /// </remarks>
        private bool loaded;

        /// <summary>
        /// The opengl initialized command.
        /// </summary>
        private DelegateCommand initializedCommand;

        /// <summary>
        /// Gets the command to be executed when opengl is initialized.
        /// </summary>
        public ICommand SceneInitializedCommand
        {
            get
            {
                if (initializedCommand == null)
                {
                    initializedCommand = new DelegateCommand(Initialized);
                }

                return initializedCommand;
            }
        }

        /// <summary>
        /// Initialize the scene.
        /// </summary>
        private void Initialized(object sender)
        {
            // Each time we switch from GameView to SceneView, the SceneView is loaded again.
            // We don't want to be initializing each scene every time. Only do this once.
            if (!loaded)
            {
                GraphicsProperties properties = sender as GraphicsProperties;
                SetupViewport(properties.Width, properties.Height);

                // Initialize all the scenes.
                foreach (Scene scene in SceneList)
                {
                    scene.Initialize();
                }
                loaded = true;
            }
        }

        #endregion

        /// <summary>
        /// The render command.
        /// </summary>
        private DelegateCommand renderCommand;

        /// <summary>
        /// Gets the command to be executed when rendering is to occur.
        /// </summary>
        public ICommand RenderCommand
        {
            get
            {
                if (renderCommand == null)
                {
                    renderCommand = new DelegateCommand(Render);
                }

                return renderCommand;
            }
        }

        /// <summary>
        /// Render the scene.
        /// </summary>
        private void Render(object sender)
        {
            foreach (Scene scene in SceneList)
            {
                scene.Render();
            }
        }

        /// <summary>
        /// The update command.
        /// </summary>
        private DelegateCommand updateCommand;

        /// <summary>
        /// Gets the command to be executed when update is to occur.
        /// </summary>
        public ICommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new DelegateCommand(Update);
                }

                return updateCommand;
            }
        }

        /// <summary>
        /// Update the scene.
        /// </summary>
        private void Update(object sender)
        {
            foreach (Scene scene in SceneList)
            {
                scene.Update();
            }
        }

        /// <summary>
        /// The resized command.
        /// </summary>
        private DelegateCommand resizedCommand;

        /// <summary>
        /// Gets the command to be executed when rendering is to occur.
        /// </summary>
        public ICommand ResizedCommand
        {
            get
            {
                if (resizedCommand == null)
                {
                    resizedCommand = new DelegateCommand(Resized);
                }

                return resizedCommand;
            }
        }

        /// <summary>
        /// Resize the scene.
        /// </summary>
        private void Resized(object sender)
        {
            GraphicsProperties properties = sender as GraphicsProperties;
            SetupViewport(properties.Width, properties.Height);
        }

        /// <summary>
        /// Set the viewport.
        /// </summary>
        private void SetupViewport(int width, int height)
        {
            // Set the view port.
            GL.Viewport(0, 0, width, height);

            // Create the perspective field of view matrix.
            float aspectRatio = (width / (float)height);
            float fov = 1f;
            float near = 1.0f;
            float far = 1000.0f;

            Matrix4 perspectiveMatrix =
               Matrix4.CreatePerspectiveFieldOfView(fov, aspectRatio, near, far);

            foreach (Scene scene in SceneList)
            {
                scene.ViewProjectionMatrix = perspectiveMatrix;
            }

            // Set the matrix mode and load the matrix.
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMatrix);
        }
    }
}
