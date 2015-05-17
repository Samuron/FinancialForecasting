using System.Collections.Generic;

namespace FinancialForecasting.Equation
{
    public static class BalanceExtensions
    {
        public static double[] ToParameters(this IReadOnlyList<double> balance)
        {
            return new[]
            {
                balance[5]/balance[4],
                (balance[5] - balance[0])/balance[2],
                balance[2]/(balance[7] + balance[8]),
                balance[1]/balance[7],
                balance[9]/((balance[3] + balance[4])/2),
                balance[12]/((balance[3] + balance[4])/2)
            };
        }
    }
}