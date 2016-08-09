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

            // Setup the file menu.
            List<string> fileMenuItems = new List<string> { "New Scene", "Open Scene", "Exit" };
            foreach (string itemName in fileMenuItems)
            {
                file.Items.Add(new MenuItemViewModel { Header = itemName });
            }

            // Setup the edit menu.
            List<string> editMenuItems = new List<string> { "Preferences" };
            foreach (string itemName in editMenuItems)
            {
                edit.Items.Add(new PreferencesMenuItemViewModel { Header = itemName });
            }

            // Setup the different views e.g. Scene and Game.
            foreach (var dockWindow in dockWindows)
            {
                view.Items.Add(GetMenuItemViewModel(dockWindow));
            }

            // Setup the game object menu.
            List<string> gameObjectMenuItems = new List<string> { "Create Empty", "3D Object", "2D Object", "Light", "Camera" };

            // Create Empty.
            var viewModel = new MenuItemViewModel { Header = "Create Empty" };
            gameObject.Items.Add(viewModel);

            // 3D Object.
            viewModel = new MenuItemViewModel { Header = "3D Object" };
            gameObject.Items.Add(viewModel);

            // 3D Object -> Sphere.
            viewModel.Items.Add(new CreateGameObjectMenuItemViewModel("Sphere") { Header = "Sphere" });

            // 3D Object -> Cube.
            viewModel.Items.Add(new CreateGameObjectMenuItemViewModel("Cube") { Header = "Cube" });

            // 3D Object -> Torus.
            viewModel.Items.Add(new CreateGameObjectMenuItemViewModel("Torus") { Header = "Torus" });

            // 2D Object.
            viewModel = new MenuItemViewModel { Header = "2D Object" };
            gameObject.Items.Add(viewModel);

            // Light.
            viewModel = new MenuItemViewModel { Header = "Light" };
            gameObject.Items.Add(viewModel);

            // Camera.
            viewModel = new MenuItemViewModel { Header = "Camera" };
            gameObject.Items.Add(viewModel);


            // Setup the help menu with a sub-item;
            List<string> helpMenuItems = new List<string> { "About" };
            foreach (string itemName in helpMenuItems)
            {
                MenuItemViewModel about = new MenuItemViewModel { Header = itemName };

                about.Items.Add(new HelpMenuItemViewModel { Header = "Help SubItem" });
                help.Items.Add(about);
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
