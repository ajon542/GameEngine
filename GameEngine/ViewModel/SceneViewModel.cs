using System.Collections.Generic;
using System.Windows.Input;

using GameEngine.Core;

namespace GameEngine.ViewModel
{
    /// <summary>
    /// ViewModel for the Scene.
    /// </summary>
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
            SceneList = new List<Scene> { new SceneA() };
        }

        #region Scene Loaded Command

        private DelegateCommand sceneLoadedCommand;
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

        private bool loaded;

        private void SceneLoaded()
        {
            // Each time we switch from GameView to SceneView, the SceneView is loaded again.
            // We don't want to be initializing each scene every time. Only do this once.
            // TODO: Hook up event handlers for things like Loaded, Render and handle the
            // rendering in here. That way the view and view model aren't both using the SceneList.
            // I think we don't want the scene list in the view at all, but not sure yet.
            if(!loaded)
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
