using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FinancialForecasting.Desktop.Annotations;
using FinancialForecasting.Desktop.Extensions;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.ViewModels
{
    public class SolvingViewModel : INotifyPropertyChanged
    {
        private bool _isFlyoutOpen;
        private ModelErrors _modelErrors;
        private string _result;

        public SolvingViewModel()
        {
            Nodes = new ObservableCollection<EquationNodeModel>
            {
                new EquationNodeModel("Коефіцієнт маневреності", "Км"),
                new EquationNodeModel("Коефіцієнт абсолютної ліквідності", "Кал"),
                new EquationNodeModel("Коефіцієнт покриття", "Кп"),
                new EquationNodeModel("Коефіцієнт швидкої ліквідності", "Кшл"),
                new EquationNodeModel("Коефіцієнт забезпечення власними коштами", "Кз"),
                new EquationNodeModel("Константа", "C") {IsVisible = false},
                new EquationNodeModel("Коефіцієнт платоспроможності", "Кпс") {IsResult = true}
            };
            foreach (var equationNode in Nodes)
                equationNode.PropertyChanged += NotifyResultChanged;
            Result = EquationFormatter.Format(Nodes);
            SolveCommand = new DelegateCommand(Solve);
            PrepareForecastCommand = new DelegateCommand(PrepareForecastData);
            ForecastCommand = new DelegateCommand(Forecast);
        }

        public DelegateCommand ForecastCommand { get; }

        public ObservableCollection<EquationNodeModel> Nodes { get; }

        public DelegateCommand SolveCommand { get; }

        public DelegateCommand PrepareForecastCommand { get; }

        public string Result
        {
            get { return _result; }
            set
            {
                if (value == _result)
                    return;
                _result = value;
                OnPropertyChanged();
            }
        }

        public ModelErrors ModelErrors
        {
            get { return _modelErrors; }
            set
            {
                _modelErrors = value;
                OnPropertyChanged();
            }
        }

        public bool IsFlyoutOpen
        {
            get { return _isFlyoutOpen; }
            set
            {
                _isFlyoutOpen = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Forecast(object parameter)
        {
            var resultNode = Nodes.First(x => x.IsResult);
            var forecast = Nodes.Where(x => x.IsEnabled).Select(x => x.GetForecast()).Sum();
            Result = $"{resultNode.ShortName}={forecast.ToString("0.0000")} ({ BankruptcyDecision.GetDecision(forecast)})";
        }

        private void NotifyResultChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Contains("Enabled"))
                foreach (var equationNode in Nodes)
                    equationNode.Factor = null;
            Result = EquationFormatter.Format(Nodes.Where(x => x.IsEnabled).ToList());
        }

        private void Solve(object parameter)
        {
            var data = (MigrationViewModel) parameter;
            var enabledEnterprises = new HashSet<string>(data.Enterprises.Where(x => x.IsEnabled).Select(x => x.Id));
            var indices =
                data.Indices.AsParallel()
                    .Where(x => enabledEnterprises.Contains(x.EnterpriseId))
                    .Select(x => x.ToArray())
                    .ToList();

            var equationBuilder = new EquationSolver(Nodes);
            var result = equationBuilder.Solve(indices);

            var elementIndex = 0;
            foreach (var node in Nodes)
            {
                if (!node.IsEnabled)
                {
                    elementIndex++;
                    continue;
                }
                node.Factor = result[elementIndex];
                elementIndex++;
                if (node.IsK1Enabled)
                {
                    node.FactorK1 = result[elementIndex];
                    elementIndex++;
                }
                if (node.IsK2Enabled)
                {
                    node.FactorK2 = result[elementIndex];
                    elementIndex++;
                }
                if (node.IsK3Enabled)
                {
                    node.FactorK3 = result[elementIndex];
                    elementIndex++;
                }
            }

            ModelErrors = new ModelErrorCalculator(Nodes).Calculate(indices);
        }

        private void PrepareForecastData(object obj)
        {
            IsFlyoutOpen = true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}