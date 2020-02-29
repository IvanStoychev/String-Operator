using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Frontend
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string output;

        public string Output
        {
            get => output;
            set
            {
                if (output != value)
                {
                    output = value;
                    OnPropertyChanged("Output");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
