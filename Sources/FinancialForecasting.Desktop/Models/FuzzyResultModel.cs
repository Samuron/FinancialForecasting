using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FinancialForecasting.Desktop.Annotations;
using FinancialForecasting.Desktop.Extensions;
using FinancialForecasting.Equation;

namespace FinancialForecasting.Desktop.Models
{
    public class FuzzyResultModel : INotifyPropertyChanged
    {
        private double _factor;
        private string _risk;

        public FuzzyResultModel(Tuple<double, Risk> result)
        {
            Factor = result.Item1;
            Risk = result.Item2.FormatRisk();
        }

        public string Risk
        {
            get { return _risk; }
            set
            {
                _risk = value;
                OnPropertyChanged();
            }
        }

        public double Factor
        {
            get { return _factor; }
            set
            {
                _factor = value;
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