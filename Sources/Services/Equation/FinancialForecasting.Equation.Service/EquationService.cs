using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FinancialForecasting.Equation.DataContracts;
using LMDotNet;

namespace FinancialForecasting.Equation
{
    [ServiceBehavior(UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class EquationService : IEquationService
    {
        public double[] Solve(IReadOnlyList<EquationNode> nodes, IReadOnlyList<double[]> indices)
        {
            var indexList = nodes.FormatIndexList(indices);
            var solver = new LMSolver();
            var optimizationResult =
                solver.Minimize(
                    (p, r) =>
                    {
                        indexList.AsParallel()
                            .Select((x, i) => new {Result = nodes.Calculate(x, p), Row = i})
                            .ForAll(x => r[x.Row] = x.Result);
                    },
                    new double[indexList.First().Length],
                    indexList.Count);

            return optimizationResult.OptimizedParameters;
        }

        public ModelErrors Calculate(IReadOnlyList<double[]> indices)
        {
            throw new System.NotImplementedException();
        }
    }
}