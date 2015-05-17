using System.Collections.Generic;
using System.ServiceModel;
using FinancialForecasting.Equation.DataContracts;

namespace FinancialForecasting.Equation
{
    [ServiceContract]
    public interface IEquationService
    {
        [OperationContract]
        double[] Solve(IReadOnlyList<EquationNode> nodes, IReadOnlyList<double[]> indices);

        [OperationContract]
        ModelErrors Calculate(IReadOnlyList<double[]> indices);
    }
}