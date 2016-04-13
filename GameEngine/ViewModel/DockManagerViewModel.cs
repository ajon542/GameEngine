using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GameEngine.ViewModel
{
    public class DockManagerViewModel
    {
        /// <summary>Gets a collection of all visible documents</summary>
        public ObservableCollection<DockWindowViewModel> Documents { get; private set; }

        public ObservableCollection<object> Anchorables { get; private set; }

        public DockManagerViewModel(IEnumerable<DockWindowViewModel> dockWindowViewModels)
        {
            Documents = new ObservableCollection<DockWindowViewModel>();
            Anchorables = new ObservableCollection<object>();

            foreach (var document in dockWindowViewModels)
            {
                document.PropertyChanged += DockWindowViewModel_PropertyChanged;
                if (!document.IsClosed)
                {
                    Documents.Add(document);
                }
            }
        }

        private void DockWindowViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DockWindowViewModel document = sender as DockWindowViewModel;

            if (e.PropertyName == "IsClosed")
            {
                if (!document.IsClosed)
                {
                    Documents.Add(document);
                }
                else
                {
                    Documents.Remove(document);
                }
            }
        }
    }
}
