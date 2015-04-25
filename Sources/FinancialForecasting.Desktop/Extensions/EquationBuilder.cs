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

        public double Calculate(EnterpriseIndexDto t, IReadOnlyList<double> p)
        {
            return p[0]*t.X1*_nodes[0].Weight() + p[1]*t.X2*_nodes[1].Weight() + p[2]*t.X3*_nodes[2].Weight() +
                   p[3]*t.X4*_nodes[3].Weight() + p[4]*t.X5*_nodes[4].Weight() + p[5]*_nodes[5].Weight() - t.Y;
        }

        public double[] Solve(IEnumerable<EnterpriseIndexDto> indices)
        {
            var indexList = indices.AsParallel().ToList();
            var solver = new LMSolver();
            var result = solver.Minimize((p, r) =>
            {
                for (var i = 0; i < indexList.Count; i++)
                {
                    var t = indexList[i];
                    r[i] = Calculate(t, p);
                }
            },
                new[] {0.0, 0.0, 0.0, 0.0, 0.0, 0.0},
                indexList.Count);

            return result.OptimizedParameters;
        }
    }
}