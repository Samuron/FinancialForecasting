using System;
using FinancialForecasting.Equation;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class RiskExtensions
    {
        public static string FormatRisk(this Risk risk)
        {
            var result = "Ризик: ";
            switch (risk)
            {
                case Risk.VeryLow:
                    result += "Дуже низький";
                    break;
                case Risk.Low:
                    result += "Низький";
                    break;
                case Risk.Average:
                    result += "Середній";
                    break;
                case Risk.High:
                    result += "Високий";
                    break;
                case Risk.VeryHigh:
                    result += "Дуже високий";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(risk), risk, null);
            }
            return result;
        }
    }
}