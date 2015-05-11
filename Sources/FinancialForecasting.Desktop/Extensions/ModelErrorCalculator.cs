using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Desktop.Extensions
{
    public class ModelErrorCalculator
    {
        private readonly EquationBuilder _equationBuilder;
        private readonly EquationNodeModel[] _nodes;
        private RegressionIndexListFormatter _regressionIndexListFormatter;

        public ModelErrorCalculator(params EquationNodeModel[] nodes)
        {
            _nodes = nodes;
            _regressionIndexListFormatter = new RegressionIndexListFormatter(nodes);
            _equationBuilder = new EquationBuilder(nodes);
        }

        public ModelErrors Calculate(IReadOnlyCollection<EnterpriseIndexDto> indices)
        {
            var yAverage = indices.Select(x => x.Y).Average();
            var factors = _nodes.Factors();

            var epsilons = from index in indices.AsParallel()
                let givenValue = index.Y
                let calculatedValue = _equationBuilder.Calculate(factors, index.ToArray())
                let e = givenValue - calculatedValue
                select new {E = e, E_ = givenValue - yAverage};

            var determination = 1 -
                                epsilons.Aggregate(0.0, (current, e) => current - e.E*e.E)/
                                epsilons.Aggregate(0.0, (current, e) => current - e.E_*e.E_);
            var eSquared = epsilons.Aggregate(0.0, (current, e) => current + e.E*e.E);
            return new ModelErrors(determination, eSquared);
        }
    }

    public class ModelErrors
    {
        public ModelErrors(double r, double e)
        {
            R = r;
            E = e;
        }

        public double R { get; }

        public double E { get; }
    }
}