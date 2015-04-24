using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FinancialForecasting.Desktop.Annotations;
using FinancialForecasting.Desktop.Extensions;
using FinancialForecasting.Migration.DataContracts;
using LMDotNet;

namespace FinancialForecasting.Desktop.ViewModels
{
    public class SolvingViewModel : INotifyPropertyChanged
    {
        private bool _isX1Enabled;
        private bool _isX2Enabled;
        private bool _isX3Enabled;
        private bool _isX4Enabled;
        private bool _isX5Enabled;
        private bool _isCEnabled;

        public SolvingViewModel()
        {
            SolveCommand = new DelegateCommand(Solve);
        }

        public String Result
        {
            get
            {
                return ExpressionFormatter.FormatWithConst(IsCEnabled, IsX1Enabled, IsX2Enabled, IsX3Enabled,
                    IsX4Enabled, IsX5Enabled);
            }
        }

        public DelegateCommand SolveCommand { get; set; }

        public Boolean IsX1Enabled
        {
            get { return _isX1Enabled; }
            set
            {
                if (value == _isX1Enabled)
                    return;
                _isX1Enabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Result));
            }
        }

        public Boolean IsX2Enabled
        {
            get { return _isX2Enabled; }
            set
            {
                if (value == _isX2Enabled)
                    return;
                _isX2Enabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Result));
            }
        }

        public Boolean IsX3Enabled
        {
            get { return _isX3Enabled; }
            set
            {
                if (value == _isX3Enabled)
                    return;
                _isX3Enabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Result));
            }
        }

        public Boolean IsX4Enabled
        {
            get { return _isX4Enabled; }
            set
            {
                if (value == _isX4Enabled)
                    return;
                _isX4Enabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Result));
            }
        }

        public Boolean IsX5Enabled
        {
            get { return _isX5Enabled; }
            set
            {
                if (value == _isX5Enabled)
                    return;
                _isX5Enabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Result));
            }
        }

        public Boolean IsCEnabled
        {
            get { return _isCEnabled; }
            set
            {
                if (value == _isCEnabled)
                    return;
                _isCEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Result));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Solve(object parameter)
        {
            var collection = ((IEnumerable<EnterpriseIndexDto>) parameter).AsParallel().ToArray();
            var solver = new LMSolver();
            var result = solver.Minimize((p, r) =>
            {
                for (var i1 = 0; i1 < collection.Length; i1++)
                {
                    var t1 = collection[i1];
                    r[i1] = -t1.X1 + p[0]*t1.X2 + p[1]*t1.X3 + p[2]*t1.X4 + p[3]*t1.X5 + p[4]*t1.X6 + p[5];
                }
            }, new[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0}, collection.Length);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}