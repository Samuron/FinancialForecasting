using System.ComponentModel;
using System.Runtime.CompilerServices;
using FinancialForecasting.Desktop.Annotations;

namespace FinancialForecasting.Desktop.Models
{
    public class EnterpriseModel : INotifyPropertyChanged
    {
        private bool _isEnabled;

        public string Id { get; set; }

        public string Name { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}