using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Migration.DataContracts;
using LMDotNet;

namespace FinancialForecasting.Desktop.Extensions
{
    public class EquationBuilder
    {
        private readonly IReadOnlyList<EquationNodeModel> _nodes;

        public EquationBuilder(params EquationNodeModel[] nodes)
        {
            _nodes = nodes;
        }

        public double Calculate(double[] t, double[] p)
        {
            var result = -t.Last();
            var elementIndex = 0;
            foreach (var node in _nodes)
            {
                if (!node.IsEnabled)
                {
                    elementIndex++;
                    continue;
                }
                result += p[elementIndex]*t[elementIndex];
                elementIndex++;
                if (node.IsK1Enabled)
                {
                    result += p[elementIndex]*t[elementIndex];
                    elementIndex++;
                }
                if (node.IsK2Enabled)
                {
                    result += p[elementIndex]*t[elementIndex];
                    elementIndex++;
                }
                if (node.IsK3Enabled)
                {
                    result += p[elementIndex]*t[elementIndex];
                    elementIndex++;
                }
            }
            return result;
        }

        public double[] Solve(IReadOnlyList<EnterpriseIndexDto> indices)
        {
            var indexList = FormatIndexList(indices);
            var solver = new LMSolver();
            var result = solver.Minimize((p, r) =>
            {
                for (var i = 0; i < indexList.Count; i++)
                {
                    r[i] = Calculate(indexList[i], p);
                }
            },
                new double[indexList.First().Length - 1],
                indexList.Count);

            return result.OptimizedParameters;
        }

        private List<double[]> FormatIndexList(IReadOnlyList<EnterpriseIndexDto> indices)
        {
            var rowsToSkip = _nodes.Max(x => x.RegressionDepth());
            var elementsInRow = _nodes.Sum(node => node.RequiredParamsCount()) + 1;
            var result = new List<double[]>();
            for (var i = rowsToSkip; i < indices.Count; i++)
            {
                var elements = new double[elementsInRow];
                var row = indices[i].ToArray();
                var elementIndex = 0;
                for (var j = 0; j < _nodes.Count; j++)
                {
                    elements[elementIndex] = row[j];
                    elementIndex++;
                    if (_nodes[j].IsK1Enabled)
                    {
                        elements[elementIndex] = indices[i - 1].ToArray()[j];
                        elementIndex++;
                    }
                    if (_nodes[j].IsK2Enabled)
                    {
                        elements[elementIndex] = indices[i - 2].ToArray()[j];
                        elementIndex++;
                    }
                    if (_nodes[j].IsK3Enabled)
                    {
                        elements[elementIndex] = indices[i - 3].ToArray()[j];
                        elementIndex++;
                    }
                }
                elements[elements.Length - 2] = row[row.Length - 2];
                elements[elements.Length - 1] = row[row.Length - 1];
                result.Add(elements);
            }
            return result;
        }
    }
}