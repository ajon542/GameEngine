using System.Collections.ObjectModel;

namespace GameEngine.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the collection of view-models to display in the view.
        /// </summary>
        public ObservableCollection<ViewModelBase> Settings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Create the view-models.
            Settings = new ObservableCollection<ViewModelBase>();

            Settings.Add(new SceneViewModel());
            Settings.Add(new GameViewModel());
        }
    }
}
