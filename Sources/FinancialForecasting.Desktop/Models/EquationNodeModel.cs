using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FinancialForecasting.Desktop.Annotations;

namespace FinancialForecasting.Desktop.Models
{
    public class EquationNodeModel : INotifyPropertyChanged
    {
        private double? _factor;
        private bool _isEnabled;
        private bool _isK1Enabled;
        private bool _isK2Enabled;
        private bool _isK3Enabled;

        public EquationNodeModel(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
            IsVisible = true;
            _isEnabled = true;
        }

        public string Name { get; }

        public string ShortName { get; }

        public bool IsVisible { get; set; }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                Factor = null;
                OnPropertyChanged();
            }
        }

        public bool IsDefined => Factor != null;

        public double? Factor
        {
            get { return _factor; }
            set
            {
                _factor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDefined));
            }
        }

        public bool IsK1Enabled
        {
            get { return _isK1Enabled; }
            set
            {
                _isK1Enabled = value;
                Factor = null;
                OnPropertyChanged();
            }
        }

        public bool IsK2Enabled
        {
            get { return _isK2Enabled; }
            set
            {
                _isK2Enabled = value;
                Factor = null;
                OnPropertyChanged();
            }
        }

        public bool IsK3Enabled
        {
            get { return _isK3Enabled; }
            set
            {
                _isK3Enabled = value;
                Factor = null;
                OnPropertyChanged();
            }
        }

        public double FactorK1 { get; set; }

        public double FactorK2 { get; set; }

        public double FactorK3 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int RegressionDepth()
        {
            if (_isK1Enabled)
                return 1;
            if (_isK2Enabled)
                return 2;
            if (_isK3Enabled)
                return 3;
            return 0;
        }

        public int RequiredParamsCount()
        {
            return Convert.ToInt32(_isK1Enabled) + Convert.ToInt32(_isK2Enabled) + Convert.ToInt32(_isK3Enabled) + 1;
        }
    }
}