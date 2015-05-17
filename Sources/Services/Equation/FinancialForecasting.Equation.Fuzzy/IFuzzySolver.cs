using System;
using System.Collections.Generic;

namespace FinancialForecasting.Equation
{
    public interface IFuzzySolver
    {
        Tuple<double, Risk> Solve(IReadOnlyList<double> balance);
    }
}