using System.Collections.Generic;
using System.Windows.Input;

using GameEngine.Core;

using Newtonsoft.Json;

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
            SceneList = new List<Scene> { new SceneA() };

            GameObject root = new GameObject("Root");
            GameObject c1 = new GameObject("C1");
            GameObject c2 = new GameObject("C2");
            GameObject c3 = new GameObject("C3");

            root.AddChild(c1);
            root.AddChild(c2);
            root.AddChild(c3);

            string output = JsonConvert.SerializeObject(root, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            // This almost deserializes correctly. We would just need to traverse the tree
            // and set all the "parent" references.
            GameObject loadedRoot = JsonConvert.DeserializeObject<GameObject>(output, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
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
