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
        public ObservableCollection<EquationNode> Nodes { get; }

        public SolvingViewModel()
        {
            SolveCommand = new DelegateCommand(Solve);
            Nodes = new ObservableCollection<EquationNode>
            {
                new EquationNode("X1"),
                new EquationNode("X2"),
                new EquationNode("X3"),
                new EquationNode("X4"),
                new EquationNode("X5"),
                new EquationNode("C"),
            };
            foreach (var equationNode in Nodes)
            {
                equationNode.PropertyChanged += NotifyResultChanged;
            }
        }

        public DelegateCommand SolveCommand { get; }

        public string Result => EquationFormatter.Format(Nodes.ToArray());

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyResultChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Result));
        }

        private void Solve(object parameter)
        {
            var data = (MigrationViewModel) parameter;
            var enabledEnterprises = new HashSet<string>(data.Enterprises.Where(x => x.IsEnabled).Select(x => x.Id));
            var indices = data.Indices.Where(x => enabledEnterprises.Contains(x.EnterpriseId));
            var equationBuilder = new EquationBuilder(Nodes.ToArray());
            var result = equationBuilder.Solve(indices);

            for (int i = 0; i < result.Length; i++)
            {
                Nodes[i].Factor = result[i];
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}