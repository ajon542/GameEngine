using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using NLog;

namespace GameEngine.ViewModel
{
    public class GameObjectData
    {
        private readonly List<GameObjectData> children = new List<GameObjectData>();
        
        public string Name { get; set; }
        public Guid Guid { get; set; }

        public IList<GameObjectData> Children
        {
            get { return children; }
        }
    }

    /// <summary>
    /// A UI-friendly wrapper around a Person object.
    /// </summary>
    public class GameObjectViewModel : ViewModelBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ReadOnlyCollection<GameObjectViewModel> children;
        private readonly GameObjectViewModel parent;
        private readonly GameObjectData person;

        private bool isExpanded;
        private bool isSelected;

        public ReadOnlyCollection<GameObjectViewModel> Children
        {
            get { return children; }
        }

        public string Name
        {
            get { return person.Name; }
        }

        public GameObjectViewModel(GameObjectData person)
            : this(person, null)
        {
        }

        private GameObjectViewModel(GameObjectData person, GameObjectViewModel parent)
        {
            this.person = person;
            this.parent = parent;

            children = new ReadOnlyCollection<GameObjectViewModel>(
                    (from child in person.Children
                     select new GameObjectViewModel(child, this))
                     .ToList<GameObjectViewModel>());
        }

        #region Presentation Members

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (isExpanded && parent != null)
                    parent.IsExpanded = true;
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    logger.Log(LogLevel.Info, "{0} isSelected {1}", person.Name, value);

                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        #region Parent

        public GameObjectViewModel Parent
        {
            get { return parent; }
        }

        #endregion // Parent

        #endregion // Presentation Members
    }

    /// <summary>
    /// This is the view-model of the UI. It provides a data source
    /// for the TreeView (the FirstGeneration property).
    /// </summary>
    public class GameObjectTreeViewModel
    {
        private readonly ReadOnlyCollection<GameObjectViewModel> firstGeneration;
        private readonly GameObjectViewModel rootPersonViewModel;

        /// <summary>
        /// Returns a read-only collection containing the first person 
        /// in the family tree, to which the TreeView can bind.
        /// </summary>
        public ReadOnlyCollection<GameObjectViewModel> FirstGeneration
        {
            get { return firstGeneration; }
        }

        public GameObjectTreeViewModel(GameObjectData rootPerson)
        {
            rootPersonViewModel = new GameObjectViewModel(rootPerson);

            firstGeneration = new ReadOnlyCollection<GameObjectViewModel>(
                new GameObjectViewModel[] 
                { 
                    rootPersonViewModel
                });
        }
    }

    public class HierarchyViewModel : DockWindowViewModel
    {
        public GameObjectTreeViewModel GameObjectTree { get; set; }

        public HierarchyViewModel()
        {
            GameObjectTree = new GameObjectTreeViewModel(GetGameObjectTree());
        }

        public static GameObjectData GetGameObjectTree()
        {
            // In a real app this method would access a database.
            return new GameObjectData
            {
                Name = "David Weatherbeam",
                Children =
                {
                    new GameObjectData
                    {
                        Name="Alberto Weatherbeam",
                        Children=
                        {
                            new GameObjectData
                            {
                                Name="Zena Hairmonger",
                                Children=
                                {
                                    new GameObjectData
                                    {
                                        Name="Sarah Applifunk",
                                    }
                                }
                            },
                            new GameObjectData
                            {
                                Name="Jenny van Machoqueen",
                                Children=
                                {
                                    new GameObjectData
                                    {
                                        Name="Nick van Machoqueen",
                                    },
                                    new GameObjectData
                                    {
                                        Name="Matilda Porcupinicus",
                                    },
                                    new GameObjectData
                                    {
                                        Name="Bronco van Machoqueen",
                                    }
                                }
                            }
                        }
                    },
                    new GameObjectData
                    {
                        Name="Komrade Winkleford",
                        Children=
                        {
                            new GameObjectData
                            {
                                Name="Maurice Winkleford",
                                Children=
                                {
                                    new GameObjectData
                                    {
                                        Name="Divinity W. Llamafoot",
                                    }
                                }
                            },
                            new GameObjectData
                            {
                                Name="Komrade Winkleford, Jr.",
                                Children=
                                {
                                    new GameObjectData
                                    {
                                        Name="Saratoga Z. Crankentoe",
                                    },
                                    new GameObjectData
                                    {
                                        Name="Excaliber Winkleford",
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
