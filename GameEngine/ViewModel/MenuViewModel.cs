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

        private readonly MenuItemViewModel ViewMenuItemViewModel;

        public MenuViewModel(IEnumerable<DockWindowViewModel> dockWindows)
        {
            var view = ViewMenuItemViewModel = new MenuItemViewModel { Header = "Views" };

            foreach (var dockWindow in dockWindows)
            {
                view.Items.Add(GetMenuItemViewModel(dockWindow));
            }

            var items = new List<MenuItemViewModel>();
            items.Add(view);
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
