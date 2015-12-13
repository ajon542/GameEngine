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

        private void SceneLoaded()
        {
        }

        #endregion
    }
}
