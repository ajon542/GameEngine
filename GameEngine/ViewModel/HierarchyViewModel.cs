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
    public class Person
    {
        readonly List<Person> _children = new List<Person>();
        public IList<Person> Children
        {
            get { return _children; }
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// A UI-friendly wrapper around a Person object.
    /// </summary>
    public class GameObjectViewModel : ViewModelBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ReadOnlyCollection<GameObjectViewModel> children;
        private readonly GameObjectViewModel parent;
        private readonly Person person;

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

        public GameObjectViewModel(Person person)
            : this(person, null)
        {
        }

        private GameObjectViewModel(Person person, GameObjectViewModel parent)
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

        public GameObjectTreeViewModel(Person rootPerson)
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
        public GameObjectTreeViewModel FamilyTree { get; set; }

        public HierarchyViewModel()
        {
            FamilyTree = new GameObjectTreeViewModel(GetFamilyTree());
        }

        public static Person GetFamilyTree()
        {
            // In a real app this method would access a database.
            return new Person
            {
                Name = "David Weatherbeam",
                Children =
                {
                    new Person
                    {
                        Name="Alberto Weatherbeam",
                        Children=
                        {
                            new Person
                            {
                                Name="Zena Hairmonger",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Sarah Applifunk",
                                    }
                                }
                            },
                            new Person
                            {
                                Name="Jenny van Machoqueen",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Nick van Machoqueen",
                                    },
                                    new Person
                                    {
                                        Name="Matilda Porcupinicus",
                                    },
                                    new Person
                                    {
                                        Name="Bronco van Machoqueen",
                                    }
                                }
                            }
                        }
                    },
                    new Person
                    {
                        Name="Komrade Winkleford",
                        Children=
                        {
                            new Person
                            {
                                Name="Maurice Winkleford",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Divinity W. Llamafoot",
                                    }
                                }
                            },
                            new Person
                            {
                                Name="Komrade Winkleford, Jr.",
                                Children=
                                {
                                    new Person
                                    {
                                        Name="Saratoga Z. Crankentoe",
                                    },
                                    new Person
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
