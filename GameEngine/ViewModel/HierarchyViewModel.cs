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
    public class PersonViewModel : INotifyPropertyChanged
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Data

        readonly ReadOnlyCollection<PersonViewModel> _children;
        readonly PersonViewModel _parent;
        readonly Person _person;

        bool _isExpanded;
        bool _isSelected;

        #endregion // Data

        #region Constructors

        public PersonViewModel(Person person)
            : this(person, null)
        {
        }

        private PersonViewModel(Person person, PersonViewModel parent)
        {
            _person = person;
            _parent = parent;

            _children = new ReadOnlyCollection<PersonViewModel>(
                    (from child in _person.Children
                     select new PersonViewModel(child, this))
                     .ToList<PersonViewModel>());
        }

        #endregion // Constructors

        #region Person Properties

        public ReadOnlyCollection<PersonViewModel> Children
        {
            get { return _children; }
        }

        public string Name
        {
            get { return _person.Name; }
        }

        #endregion // Person Properties

        #region Presentation Members

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
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
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    logger.Log(LogLevel.Info, "{0} isSelected {1}", _person.Name, value);

                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        #region Parent

        public PersonViewModel Parent
        {
            get { return _parent; }
        }

        #endregion // Parent

        #endregion // Presentation Members

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }

    /// <summary>
    /// This is the view-model of the UI.  It provides a data source
    /// for the TreeView (the FirstGeneration property), a bindable
    /// SearchText property, and the SearchCommand to perform a search.
    /// </summary>
    public class FamilyTreeViewModel
    {
        #region Data

        readonly ReadOnlyCollection<PersonViewModel> _firstGeneration;
        readonly PersonViewModel _rootPerson;

        string _searchText = String.Empty;

        #endregion // Data

        #region Constructor

        public FamilyTreeViewModel(Person rootPerson)
        {
            _rootPerson = new PersonViewModel(rootPerson);

            _firstGeneration = new ReadOnlyCollection<PersonViewModel>(
                new PersonViewModel[] 
                { 
                    _rootPerson 
                });
        }

        #endregion // Constructor

        #region Properties

        #region FirstGeneration

        /// <summary>
        /// Returns a read-only collection containing the first person 
        /// in the family tree, to which the TreeView can bind.
        /// </summary>
        public ReadOnlyCollection<PersonViewModel> FirstGeneration
        {
            get { return _firstGeneration; }
        }

        #endregion // FirstGeneration

        #endregion // Properties
    }

    public class HierarchyViewModel : DockWindowViewModel
    {
        public FamilyTreeViewModel FamilyTree { get; set; }

        public HierarchyViewModel()
        {
            FamilyTree = new FamilyTreeViewModel(GetFamilyTree());
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
