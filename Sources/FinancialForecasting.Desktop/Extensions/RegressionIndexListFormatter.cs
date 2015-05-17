using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Desktop.Extensions
{
    public class RegressionIndexListFormatter
    {
        private readonly IReadOnlyList<EquationNodeModel> _nodes;

        public RegressionIndexListFormatter(IReadOnlyList<EquationNodeModel> nodes)
        {
            _nodes = nodes;
        }

        public List<double[]> FormatIndexList(IReadOnlyList<double[]> indices)
        {
            var rowsToSkip = _nodes.Max(x => x.RegressionDepth());

            return Enumerable.Range(rowsToSkip, indices.Count - rowsToSkip)
                    .Select(i => new {Index = indices[i], Row = i})
                    .Select(x => _nodes.SelectMany((node, j) =>
                    {
                        var list = new List<double> {x.Index[j]};
                        if (_nodes[j].IsK1Enabled)
                            list.Add(indices[x.Row - 1][j]);
                        if (_nodes[j].IsK2Enabled)
                            list.Add(indices[x.Row - 2][j]);
                        if (_nodes[j].IsK3Enabled)
                            list.Add(indices[x.Row - 3][j]);
                        return list;
                    }).ToArray()).ToList();
        }
    }
}