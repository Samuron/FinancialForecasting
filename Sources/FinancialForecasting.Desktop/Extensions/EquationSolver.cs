using System.Collections.Generic;
using System.Linq;
using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Migration.DataContracts;
using LMDotNet;

namespace FinancialForecasting.Desktop.Extensions
{
    public class EquationSolver
    {
        private readonly EquationBuilder _equationBuilder;
        private readonly RegressionIndexListFormatter _regressionIndexListFormatter;

        public EquationSolver(IReadOnlyList<EquationNodeModel> nodes)
        {
            _regressionIndexListFormatter = new RegressionIndexListFormatter(nodes);
            _equationBuilder = new EquationBuilder(nodes);
        }

        public double[] Solve(IReadOnlyList<double[]> indices)
        {
            var indexList = _regressionIndexListFormatter.FormatIndexList(indices);
            var solver = new LMSolver();
            var optimizationResult =
                solver.Minimize(
                    (p, r) =>
                    {
                        indexList.AsParallel()
                            .Select((x, i) => new {Result = _equationBuilder.Calculate(x, p), Row = i})
                            .ForAll(x => r[x.Row] = x.Result);
                    },
                    new double[indexList.First().Length],
                    indexList.Count);

            return optimizationResult.OptimizedParameters;
        }
    }
}