using System;
using System.Collections.Generic;

namespace FinancialForecasting.Equation
{
    public class AltmanModelSolver : IFuzzySolver
    {
        public Tuple<double, Risk> Solve(IReadOnlyList<double> balance)
        {
            var x = new[]
            {
                (balance[2] + balance[7] + balance[8])/balance[4],
                balance[12]/balance[4],
                balance[11]/balance[4],
                balance[5]/(balance[6] + balance[7] + balance[8]),
                balance[10]/balance[4]
            };

            var z = 1.2*x[0] + 1.4*x[1] + 3.3*x[2] + 0.6*x[3] + x[4];

            Risk risk;
            if (z < 1.81)
                risk = Risk.VeryHigh;
            else if ((1.81 <= z) & (z < 2.77))
                risk = Risk.High;
            else if ((2.77 <= z) & (z < 2.99))
                risk = Risk.Average;
            else if (z >= 2.99)
                risk = Risk.Low;
            else
                risk = Risk.VeryHigh;
            return Tuple.Create(z, risk);
        }
    }
}