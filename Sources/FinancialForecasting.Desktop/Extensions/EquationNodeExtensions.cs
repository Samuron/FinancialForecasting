using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EquationNodeExtensions
    {
        public static double[] Factors(this IEnumerable<EquationNodeModel> nodes)
        {
            return nodes.SelectMany(node =>
            {
                var list = new List<double> {node.Factor ?? 0.0};
                if (node.IsK1Enabled)
                    list.Add(node.FactorK1);
                if (node.IsK2Enabled)
                    list.Add(node.FactorK2);
                if (node.IsK3Enabled)
                    list.Add(node.FactorK3);
                return list;
            }).ToArray();
        }

        public static double GetForecast(this EquationNodeModel node)
        {
            var weight = node.IsResult ? 0.0 : node.IsVisible ? node.Weight : 1.0;
            var f = node.Factor*weight ?? 0.0;
            var f1 = node.FactorK1*node.WeightK1 ?? 0.0;
            var f2 = node.FactorK2*node.WeightK2 ?? 0.0;
            var f3 = node.FactorK3*node.WeightK3 ?? 0.0;
            return f + f1 + f2 + f3;
        }
    }
}