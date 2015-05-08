using FinancialForecasting.Desktop.Models;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Desktop.Extensions
{
    internal static class ModelFactory
    {
        public static EnterpriseModel ToModel(this EnterpriseDto x)
        {
            return new EnterpriseModel {IsEnabled = true, Id = x.Id, Name = x.Name};
        }
    }
}