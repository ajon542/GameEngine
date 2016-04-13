using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameEngine.ViewModel
{
    public class MenuItemViewModel : ViewModelBase
    {
        #region Properties

        public string Header { get; set; }
        public bool IsCheckable { get; set; }
        public List<MenuItemViewModel> Items { get; private set; }
        public ICommand Command { get; private set; }

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
