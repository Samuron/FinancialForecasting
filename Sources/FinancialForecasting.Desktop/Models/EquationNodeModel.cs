using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
                if (IsResult)
                    return;
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

        public bool IsResult { get; set; }

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
            var regressionCandidates = new List<int> {0};
            if (_isK1Enabled)
                regressionCandidates.Add(1);
            if (_isK2Enabled)
                regressionCandidates.Add(2);
            if (_isK3Enabled)
                regressionCandidates.Add(3);
            return regressionCandidates.Max();
        }

        public int RequiredParamsCount()
        {
            return Convert.ToInt32(_isK1Enabled) + Convert.ToInt32(_isK2Enabled) + Convert.ToInt32(_isK3Enabled) + 1;
        }
    }
}