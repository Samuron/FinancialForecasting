using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Windows.Input;
using FinancialForecasting.Desktop.Annotations;
using FinancialForecasting.Desktop.Clients;
using FinancialForecasting.Migration;
using FinancialForecasting.Migration.DataContracts;
using Microsoft.Win32;

namespace FinancialForecasting.Desktop
{
    [CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class MigrationViewModel : INotifyPropertyChanged, INotifyMigrationProgress
    {
        private readonly MigrationClient _service;
        private int _currentRow;
        private string _filePath;
        private IEnumerable<EnterpriseIndexDto> _indices;
        private int _numberOfRows;
        private ICommand _selectFileCommand;
        private ICommand _startMigrationCommand;

        public MigrationViewModel()
        {
            _service = new MigrationClient(this);
            SelectFileCommand = new DelegateCommand(SelectFile);
            StartMigrationCommand = new DelegateCommand(StartMigration);
            Indices = _service.GetIndexes();
            NumberOfRows = 100;
            CurrentRow = 0;
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

        public IEnumerable<EnterpriseIndexDto> Indices
        {
            get { return _indices; }
            set
            {
                if (Equals(value, _indices))
                    return;
                _indices = value;
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

        public ICommand StartMigrationCommand
        {
            get { return _startMigrationCommand; }
            set
            {
                if (Equals(value, _startMigrationCommand))
                    return;
                _startMigrationCommand = value;
                OnPropertyChanged();
            }
        }

        public void AcceptMaxRows(int maxRows)
        {
            NumberOfRows = maxRows;
        }

        public void AcceptCurrentRow(int rowNumber)
        {
            CurrentRow = rowNumber;
            if (CurrentRow == NumberOfRows)
                Indices = _service.GetIndexes();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void StartMigration(object obj)
        {
            var fileStream = new MemoryStream(File.ReadAllBytes(FilePath));
            if (Path.GetExtension(FilePath) == ".xls")
                _service.MigrateXls(fileStream);
            if (Path.GetExtension(FilePath) == ".xlsx")
                _service.MigrateXlsx(fileStream);
        }

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