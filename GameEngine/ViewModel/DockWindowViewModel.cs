using System.Windows.Input;

namespace GameEngine.ViewModel
{
    public abstract class DockWindowViewModel : ViewModelBase
    {
        #region Properties

        #region CloseCommand
        private ICommand closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                    closeCommand = new DelegateCommand(call => Close());
                return closeCommand;
            }
        }
        #endregion

        #region IsClosed
        private bool isClosed;
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                if (isClosed != value)
                {
                    isClosed = value;
                    OnPropertyChanged("IsClosed");
                }
            }
        }
        #endregion

        #region CanClose
        private bool canClose;
        public bool CanClose
        {
            get { return canClose; }
            set
            {
                if (canClose != value)
                {
                    canClose = value;
                    OnPropertyChanged("CanClose");
                }
            }
        }
        #endregion

        #region Title
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        #endregion

        #endregion

        public DockWindowViewModel()
        {
            this.CanClose = true;
            this.IsClosed = false;
        }

        public void Close()
        {
            this.IsClosed = true;
        }
    }
}
