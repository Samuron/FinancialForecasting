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
            var result = 0.0;
            var elementIndex = 0;
            foreach (var node in _nodes)
            {
                if (!node.IsEnabled)
                {
                    elementIndex++;
                    continue;
                }
                if (node.IsResult)
                {
                    result -= t[elementIndex];
                    elementIndex++;
                }
                else
                {
                    result += p[elementIndex] * t[elementIndex];
                    elementIndex++;
                }
                if (node.IsK1Enabled)
                {
                    result += p[elementIndex] * t[elementIndex];
                    elementIndex++;
                }
                if (node.IsK2Enabled)
                {
                    result += p[elementIndex] * t[elementIndex];
                    elementIndex++;
                }
                if (node.IsK3Enabled)
                {
                    result += p[elementIndex] * t[elementIndex];
                    elementIndex++;
                }
            }
            return result;
        }
    }
}