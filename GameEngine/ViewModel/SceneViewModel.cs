using System.Collections.Generic;
using System.Windows.Input;

using GameEngine.Core;

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
            SceneList = new List<Scene> { new Core.GameSpecific.GameScene() };
        }

        #region Scene Loaded Command

        /// <summary>
        /// Keeps track of whether the view has been loaded previously.
        /// </summary>
        /// <remarks>
        /// Each time we switch from GameView to SceneView, the SceneView is loaded again.
        /// We don't want to be initializing each scene every time. Only do this once.
        /// </remarks>
        private bool loaded;

        /// <summary>
        /// The command to execute when the scene is loaded.
        /// </summary>
        private DelegateCommand sceneLoadedCommand;

        /// <summary>
        /// Gets the command to execute when the scene is loaded.
        /// </summary>
        public ICommand SceneLoadedCommand
        {
            get
            {
                if (sceneLoadedCommand == null)
                {
                    sceneLoadedCommand = new DelegateCommand(SceneLoaded);
                }

                return sceneLoadedCommand;
            }
        }

        /// <summary>
        /// The method to execute when the scene is loaded.
        /// </summary>
        private void SceneLoaded()
        {
            // Each time we switch from GameView to SceneView, the SceneView is loaded again.
            // We don't want to be initializing each scene every time. Only do this once.
            // TODO: Hook up event handlers for things like Loaded, Render and handle the
            // rendering in here. That way the view and view model aren't both using the SceneList.
            // I think we don't want the scene list in the view at all, but not sure yet.
            if (!loaded)
            {
                // Initialize all the scenes.
                foreach (Scene scene in SceneList)
                {
                    scene.Initialize();
                }
                loaded = true;
            }
        }

        #endregion
    }
}
