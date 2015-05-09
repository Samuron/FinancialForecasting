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
        public SolvingViewModel()
        {
            SolveCommand = new DelegateCommand(Solve);
            Nodes = new ObservableCollection<EquationNodeModel>
            {
                new EquationNodeModel("���������� �����������", "��"),
                new EquationNodeModel("���������� ��������� ��������", "���"),
                new EquationNodeModel("���������� ��������", "��"),
                new EquationNodeModel("���������� ������ ��������", "���"),
                new EquationNodeModel("���������� ������������ �������� �������", "��"),
                new EquationNodeModel("���������", "C") {IsVisible = false}
            };
            foreach (var equationNode in Nodes)
                equationNode.PropertyChanged += NotifyResultChanged;
        }

        public ObservableCollection<EquationNodeModel> Nodes { get; }

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
            var indices = data.Indices.AsParallel().Where(x => enabledEnterprises.Contains(x.EnterpriseId)).ToList();
            var equationBuilder = new EquationBuilder(Nodes.ToArray());
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
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}