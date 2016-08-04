using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ViewModel
{
    public class MenuViewModel
    {
        public IEnumerable<MenuItemViewModel> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuViewModel"/> class.
        /// </summary>
        /// <param name="dockWindows">The list of dock windows.</param>
        public MenuViewModel(IEnumerable<DockWindowViewModel> dockWindows)
        {
            MenuItemViewModel file = new MenuItemViewModel { Header = "File" };
            MenuItemViewModel edit = new MenuItemViewModel { Header = "Edit" };
            MenuItemViewModel view = new MenuItemViewModel { Header = "View" };
            MenuItemViewModel gameObject = new MenuItemViewModel { Header = "GameObject" };
            MenuItemViewModel help = new MenuItemViewModel { Header = "Help" };

            foreach (var dockWindow in dockWindows)
            {
                view.Items.Add(GetMenuItemViewModel(dockWindow));
            }

            var items = new List<MenuItemViewModel>();
            items.Add(file);
            items.Add(edit);
            items.Add(view);
            items.Add(gameObject);
            items.Add(help);
            Items = items;
        }

        private MenuItemViewModel GetMenuItemViewModel(DockWindowViewModel dockWindowViewModel)
        {
            var menuItemViewModel = new MenuItemViewModel();
            menuItemViewModel.IsCheckable = true;

            menuItemViewModel.Header = dockWindowViewModel.Title;
            menuItemViewModel.IsChecked = !dockWindowViewModel.IsClosed;

            dockWindowViewModel.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "IsClosed")
                {
                    menuItemViewModel.IsChecked = !dockWindowViewModel.IsClosed;
                }
            };

            menuItemViewModel.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "IsChecked")
                {
                    dockWindowViewModel.IsClosed = !menuItemViewModel.IsChecked;
                }
            };

            return menuItemViewModel;
        }
    }
}
