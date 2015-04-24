using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using FinancialForecasting.Desktop.Annotations;
using FinancialForecasting.Desktop.Clients;
using FinancialForecasting.Migration;
using FinancialForecasting.Migration.DataContracts;
using Microsoft.Win32;

namespace FinancialForecasting.Desktop.ViewModels
{
    [CallbackBehavior(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class MigrationViewModel : INotifyPropertyChanged, INotifyMigrationProgress
    {
        private readonly MigrationClient _service;
        private int _currentRow;
        private string _filePath;
        private IEnumerable<EnterpriseIndexDto> _indices;
        private int _numberOfRows;

        public MigrationViewModel()
        {
            _service = new MigrationClient(this);
            SelectFileCommand = new DelegateCommand(SelectFile);
            StartMigrationCommand = new DelegateCommand(StartMigration, CanStartMigration);
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
                StartMigrationCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public DelegateCommand SelectFileCommand { get; }

        public DelegateCommand StartMigrationCommand { get; }

        private void StartMigration(object sender)
        {
            var fileStream = new MemoryStream(File.ReadAllBytes(FilePath));
            if (Path.GetExtension(FilePath) == ".xls")
                _service.MigrateXls(fileStream);
            if (Path.GetExtension(FilePath) == ".xlsx")
                _service.MigrateXlsx(fileStream);
        }

        private bool CanStartMigration(object sender)
        {
            return FilePath != null;
        }

        private void SelectFile(object sender)
        {
            var dialogWindow = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx",
                Title = "Please, select file with migration data"
            };
            var dialogResult = dialogWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
                FilePath = dialogWindow.FileName;
        }

        public void AcceptMaxRows(int maxRows)
        {
            NumberOfRows = maxRows;
        }

        public void AcceptCurrentRow(int rowNumber)
        {
            CurrentRow = rowNumber;
        }

        public void MigrationFinished()
        {
            Indices = _service.GetIndexes();
            CurrentRow = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}