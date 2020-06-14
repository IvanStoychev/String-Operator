using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Backend;

namespace Frontend
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string input;
        private string output;

        public string Input
        {
            get => input;
            set
            {
                if (input != value)
                {
                    input = value;
                    OnPropertyChanged("Input");
                    ExecuteCommand.RaiseCanExecuteChanged();
                }
            }
        }

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

        public RelayCommand ExecuteCommand { get; }

        public MainWindowViewModel()
        {
            ExecuteCommand = new RelayCommand(OnExecute, CanExecute);
            Input = System.IO.File.ReadAllText(@"J:\Users\Administrator\Source\Repos\Useful.Sqlite.Extensions\Useful.Sqlite.Extensions\SQLiteDataReaderExtensions.cs");
            OnExecute();
        }

        private void OnExecute()
        {
            Task.Factory.StartNew(() => Output = MasterController.GenerateGithubWikiPage(Input));
        }

        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(Input);
        }
    }
}
