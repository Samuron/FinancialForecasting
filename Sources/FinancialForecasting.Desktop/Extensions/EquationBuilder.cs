using System.Collections.Generic;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    internal class EquationBuilder
    {
        private readonly IReadOnlyCollection<EquationNodeModel> _nodes;

        public EquationBuilder(IReadOnlyCollection<EquationNodeModel> nodes)
        {
            _nodes = nodes;
        }

        public double Calculate(double[] t, double[] p)
        {
            return _nodes.Fold(0.0,
                (x, y) => x + y,
                (i, _) => 0.0,
                (i, node) => node.IsResult ? -t[i] : p[i]*t[i],
                (i, _) => p[i]*t[i],
                (i, _) => p[i]*t[i],
                (i, _) => p[i]*t[i]);
        }
    }
}