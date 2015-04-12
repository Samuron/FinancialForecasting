using System.Data.Entity;

namespace FinancialForecasting.Migration
{
    public class FinancialForecastingInitializer : DropCreateDatabaseIfModelChanges<FinancialForecastingContext>
    {
    }
}