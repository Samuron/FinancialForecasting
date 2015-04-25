using System.ComponentModel;
using System.Runtime.CompilerServices;
using FinancialForecasting.Desktop.Annotations;

namespace FinancialForecasting.Desktop.Models
{
    public class EquationNode : INotifyPropertyChanged
    {
        private double? _factor;
        private bool _isEnabled;

        public EquationNode()
        {
            _isEnabled = true;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value == _isEnabled)
                    return;
                _isEnabled = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}