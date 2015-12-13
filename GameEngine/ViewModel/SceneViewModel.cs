using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;

using GameEngine.Core;

namespace GameEngine.ViewModel
{
    public class SceneViewModel : ViewModelBase
    {
        private List<Scene> sceneList;

        public SceneViewModel()
        {
            // Setting up the scene here is too early and OpenGL hasn't initialized yet.
            // We shouldn't have terms like "too early" because that makes the system fragile.
            // We should get a notification from the OpenGLControl specifying exactly when
            // we can do our initialization. That being said, scene initialization isn't just
            // going to happen at a single point. It will happen whenever a new scene is added.
            SceneList = new List<Scene> { new SceneA() };
        }

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

        DelegateCommand initializedCommand = null;
        public ICommand InitializedCommand
        {
            get
            {
                if (initializedCommand == null)
                {
                    initializedCommand = new DelegateCommand(() => OnInitialized());
                }

                return initializedCommand;
            }
        }

        private void OnInitialized()
        {
        }
    }
}
