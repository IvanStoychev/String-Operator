using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Frontend
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private FrameworkElement currentControlView;

        public FrameworkElement CurrentControlView
        {
            get => currentControlView;
            set
            {
                if (currentControlView != value)
                {
                    currentControlView = value;
                    OnPropertyChanged("CurrentControlView");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
