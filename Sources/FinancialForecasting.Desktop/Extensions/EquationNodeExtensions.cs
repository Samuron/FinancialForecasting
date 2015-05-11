using System;
using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EquationNodeExtensions
    {
        public static double Weight(this EquationNodeModel node)
        {
            return node.IsEnabled ? 1.0 : 0.0;
        }

        public static double[] Factors(this IReadOnlyList<EquationNodeModel> nodes)
        {
            return nodes.SelectMany(node =>
            {
                var list = new List<double> {node.Factor ?? double.NaN};
                if (node.IsK1Enabled)
                    list.Add(node.FactorK1);
                if (node.IsK2Enabled)
                    list.Add(node.FactorK2);
                if (node.IsK3Enabled)
                    list.Add(node.FactorK3);
                return list;
            }).ToArray();
        }
    }
}