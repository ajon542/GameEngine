using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GameEngine.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Collection of view-models to display in the list.
        /// </summary>
        private readonly ObservableCollection<ViewModelBase> settings;

        /// <summary>
        /// Gets the collection of view-models to display in the list.
        /// </summary>
        public ObservableCollection<ViewModelBase> Settings
        {
            get { return settings; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Create the view-models.
            settings = new ObservableCollection<ViewModelBase>();

            settings.Add(new SceneViewModel());
            settings.Add(new GameViewModel());
        }
    }
}
