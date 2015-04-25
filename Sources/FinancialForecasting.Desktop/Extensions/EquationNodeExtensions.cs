using FinancialForecasting.Desktop.Models;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EquationNodeExtensions
    {
        public static double Weight(this EquationNode node)
        {
            return node.IsEnabled ? 1.0 : 0.0;
        }
    }
}