using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public MainViewModel()
        {
            // Create the view-models.
            settings = new ObservableCollection<ViewModelBase>();

            settings.Add(new SceneViewModel());
            settings.Add(new GameViewModel());
        }

        private DelegateCommand openCommand;
        public ICommand OpenCommand
        {
            get
            {
                if (openCommand == null)
                {
                    openCommand = new DelegateCommand(Open);
                }

                return openCommand;
            }
        }

        private void Open()
        {

        }

        private DelegateCommand optionsCommand;
        public ICommand OptionsCommand
        {
            get
            {
                if (optionsCommand == null)
                {
                    optionsCommand = new DelegateCommand(Options);
                }

                return optionsCommand;
            }
        }

        private void Options()
        {

        }
    }
}
