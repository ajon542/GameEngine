using System.Collections.Generic;
using System.Windows.Input;

using NLog;

namespace GameEngine.ViewModel
{
    /// <summary>
    /// Help menu view model to open the help window.
    /// </summary>
    public class HelpMenuItemViewModel : MenuItemViewModel
    {
        private DelegateCommand command;

        public override ICommand Command
        {
            get
            {
                if (command == null)
                {
                    command = new DelegateCommand(OpenWindow);
                }

                return command;
            }
        }

        private void OpenWindow(object sender)
        {
            HelpWindow window = new HelpWindow();
            window.Show();
        }
    }

    /// <summary>
    /// Preferences menu view model to open the help window.
    /// </summary>
    public class PreferencesMenuItemViewModel : MenuItemViewModel
    {
        private DelegateCommand command;

        public override ICommand Command
        {
            get
            {
                if (command == null)
                {
                    command = new DelegateCommand(OpenWindow);
                }

                return command;
            }
        }

        private void OpenWindow(object sender)
        {
            PreferencesWindow window = new PreferencesWindow();
            window.Show();
        }
    }

    public class MenuItemViewModel : ViewModelBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Properties

        public string Header { get; set; }
        public bool IsCheckable { get; set; }
        public List<MenuItemViewModel> Items { get; private set; }
        public virtual ICommand Command { get; set; }

        #region IsPressed
        private bool isPressed;
        public bool IsPressed
        {
            get { return isPressed; }
            set
            {
                if (isPressed != value)
                {
                    isPressed = value;
                    OnPropertyChanged("IsPressed");
                }
            }
        }
        #endregion

        #region IsChecked
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }
        #endregion

        #endregion

        public MenuItemViewModel()
        {
            Items = new List<MenuItemViewModel>();
        }
    }
}
