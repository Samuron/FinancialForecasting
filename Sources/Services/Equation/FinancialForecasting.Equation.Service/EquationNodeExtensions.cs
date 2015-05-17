using System;
using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Equation.DataContracts;

namespace FinancialForecasting.Equation
{
    public static class EquationNodeExtensions
    {
        public static int RegressionDepth(this EquationNode node)
        {
            var regressionCandidates = new List<int> {0};
            if (node.IsK1Enabled)
                regressionCandidates.Add(1);
            if (node.IsK2Enabled)
                regressionCandidates.Add(2);
            if (node.IsK3Enabled)
                regressionCandidates.Add(3);
            return regressionCandidates.Max();
        }

        public static T Fold<T>(this IEnumerable<EquationNode> nodes,
            T initial,
            Func<T, T, T> combiner,
            Func<int, EquationNode, T> onDisabled,
            Func<int, EquationNode, T> onEnabled,
            Func<int, EquationNode, T> onK1,
            Func<int, EquationNode, T> onK2,
            Func<int, EquationNode, T> onK3)
        {
            var current = initial;
            var elementIndex = 0;
            foreach (var node in nodes)
            {
                if (!node.IsEnabled)
                {
                    current = combiner(current, onDisabled(elementIndex++, node));
                    continue;
                }
                current = combiner(current, onEnabled(elementIndex++, node));
                if (node.IsK1Enabled)
                    current = combiner(current, onK1(elementIndex++, node));
                if (node.IsK2Enabled)
                    current = combiner(current, onK2(elementIndex++, node));
                if (node.IsK3Enabled)
                    current = combiner(current, onK3(elementIndex++, node));
            }
            return current;
        }

        public static double Calculate(this IReadOnlyList<EquationNode> nodes, double[] t, double[] p)
        {
            return nodes.Fold(0.0,
                (x, y) => x + y,
                (i, _) => 0.0,
                (i, node) => node.IsResult ? -t[i] : p[i]*t[i],
                (i, _) => p[i]*t[i],
                (i, _) => p[i]*t[i],
                (i, _) => p[i]*t[i]);
        }

        public static IReadOnlyList<double[]> FormatIndexList(
            this IReadOnlyList<EquationNode> nodes,
            IReadOnlyList<double[]> indices)
        {
            var rowsToSkip = nodes.Max(x => x.RegressionDepth());

            return
                Enumerable.Range(rowsToSkip, indices.Count - rowsToSkip)
                    .Select(i => new {Index = indices[i], Row = i})
                    .Select(x => nodes.SelectMany((node, j) =>
                    {
                        var list = new List<double> {x.Index[j]};
                        if (nodes[j].IsK1Enabled)
                            list.Add(indices[x.Row - 1][j]);
                        if (nodes[j].IsK2Enabled)
                            list.Add(indices[x.Row - 2][j]);
                        if (nodes[j].IsK3Enabled)
                            list.Add(indices[x.Row - 3][j]);
                        return list;
                    }).ToArray()).ToList();
        }
    }
}