using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Migration.DataContracts;
using LMDotNet;

namespace FinancialForecasting.Desktop.Extensions
{
    public class EquationBuilder
    {
        private readonly IReadOnlyList<EquationNode> _nodes;

        public EquationBuilder(params EquationNode[] nodes)
        {
            _nodes = nodes;
        }

        public double Calculate(double[] t, double[] p)
        {
            var result = -t.Last();
            for (var i = 0; i < t.Length - 1; i++)
            {
                result += p[i]*t[i]*_nodes[i].Weight();
            }
            return result;
        }

        public double[] Solve(IEnumerable<EnterpriseIndexDto> indices)
        {
            var indexList = indices.AsParallel().Select(x => x.ToArray()).ToList();
            var solver = new LMSolver();
            var result = solver.Minimize((p, r) =>
            {
                for (var i = 0; i < indexList.Count; i++)
                {
                    r[i] = Calculate(indexList[i], p);
                }
            },
                new double[_nodes.Count],
                indexList.Count);

            return result.OptimizedParameters;
        }
    }
}