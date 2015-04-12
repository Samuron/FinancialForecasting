using FastMapper;
using FinancialForecasting.Migration.DataContracts;
using FinancialForecasting.Migration.Entities;

namespace FinancialForecasting.Migration
{
    public static class MapperConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<EnterpriseIndex, EnterpriseIndexDto>.NewConfig();
        }
    }
}