using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FinancialForecasting.Desktop.Annotations;
using Microsoft.Win32;

namespace FinancialForecasting.Desktop
{
    public class MigrationViewModel : INotifyPropertyChanged
    {
        private int _currentRow;
        private string _filePath;
        private int _numberOfRows;
        private ICommand _selectFileCommand;

        public MigrationViewModel()
        {
            SelectFileCommand = new DelegateCommand(SelectFile);
        }

        public int NumberOfRows
        {
            get { return _numberOfRows; }
            set
            {
                if (value == _numberOfRows)
                    return;
                _numberOfRows = value;
                OnPropertyChanged();
            }
        }

        public int CurrentRow
        {
            get { return _currentRow; }
            set
            {
                if (value == _currentRow)
                    return;
                _currentRow = value;
                OnPropertyChanged();
            }
        }

        public String FilePath
        {
            get { return _filePath; }
            set
            {
                if (value == _filePath)
                    return;
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectFileCommand
        {
            get { return _selectFileCommand; }
            set
            {
                if (Equals(value, _selectFileCommand))
                    return;
                _selectFileCommand = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SelectFile(object sender)
        {
            var dialogWindow = new OpenFileDialog {DefaultExt = ".xls;.xlsx"};
            var dialogResult = dialogWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
                FilePath = dialogWindow.FileName;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}