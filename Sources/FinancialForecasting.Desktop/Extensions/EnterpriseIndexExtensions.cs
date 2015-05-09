using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Desktop.Extensions
{
    public static class EnterpriseIndexExtensions
    {
        public static double[] ToArray(this EnterpriseIndexDto index)
        {
            return new[] { index.X1, index.X2, index.X3, index.X4, index.X5, 1.0, index.Y };
        }
    }
}