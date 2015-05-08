using System.Collections.Generic;
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
        public SolvingViewModel()
        {
            SolveCommand = new DelegateCommand(Solve);
            X1Node.PropertyChanged += NotifyResultChanged;
            X2Node.PropertyChanged += NotifyResultChanged;
            X3Node.PropertyChanged += NotifyResultChanged;
            X4Node.PropertyChanged += NotifyResultChanged;
            X5Node.PropertyChanged += NotifyResultChanged;
            CNode.PropertyChanged += NotifyResultChanged;
        }

        public DelegateCommand SolveCommand { get; }

        public EquationNode X1Node { get; set; } = new EquationNode();

        public EquationNode X2Node { get; set; } = new EquationNode();

        public EquationNode X3Node { get; set; } = new EquationNode();

        public EquationNode X4Node { get; set; } = new EquationNode();

        public EquationNode X5Node { get; set; } = new EquationNode();

        public EquationNode CNode { get; set; } = new EquationNode();

        public string Result => EquationFormatter.Format(X1Node, X2Node, X3Node, X4Node, X5Node, CNode);

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
            var equationBuilder = new EquationBuilder(X1Node, X2Node, X3Node, X4Node, X5Node, CNode);
            var result = equationBuilder.Solve(indices);

            X1Node.Factor = result[0];
            X2Node.Factor = result[1];
            X3Node.Factor = result[2];
            X4Node.Factor = result[3];
            X5Node.Factor = result[4];
            CNode.Factor = result[5];
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}