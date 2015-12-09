using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.ViewModel
{
    public class SceneViewModel : ViewModelBase
    {
        private double rValue;
        private double gValue;
        private double bValue;

        public double RValue
        {
            get 
            {
                return rValue; 
            }
            set
            {
                rValue = value;
                OnPropertyChanged("RValue");
            }
        }

        public double GValue
        {
            get
            {
                return gValue;
            }
            set
            {
                gValue = value;
                OnPropertyChanged("GValue");
            }
        }

        public double BValue
        {
            get
            {
                return bValue;
            }
            set
            {
                bValue = value;
                OnPropertyChanged("BValue");
            }
        }
    }
}
