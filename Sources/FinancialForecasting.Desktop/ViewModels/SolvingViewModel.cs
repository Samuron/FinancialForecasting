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
        private ModelErrors _modelErrors;

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
            SolveCommand = new DelegateCommand(Solve);
            foreach (var equationNode in Nodes)
                equationNode.PropertyChanged += NotifyResultChanged;
        }

        public ObservableCollection<EquationNodeModel> Nodes { get; }

        public DelegateCommand SolveCommand { get; }

        public string Result => EquationFormatter.Format(Nodes.Where(x => x.IsEnabled).ToList());

        public ModelErrors ModelErrors
        {
            get { return _modelErrors; }
            set
            {
                _modelErrors = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyResultChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Contains("Enabled"))
                foreach (var equationNode in Nodes)
                    equationNode.Factor = null;
            OnPropertyChanged(nameof(Result));
        }

        private void Solve(object parameter)
        {
            var data = (MigrationViewModel) parameter;
            var enabledEnterprises = new HashSet<string>(data.Enterprises.Where(x => x.IsEnabled).Select(x => x.Id));
            var indices = data.Indices.AsParallel().Where(x => enabledEnterprises.Contains(x.EnterpriseId)).Select(x=>x.ToArray()).ToList();

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}