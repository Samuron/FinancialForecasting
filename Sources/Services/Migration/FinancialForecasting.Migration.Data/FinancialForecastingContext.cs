using System.Data.Entity;
using FinancialForecasting.Migration.Entities;

namespace FinancialForecasting.Migration
{
    public class FinancialForecastingContext : DbContext
    {
        static FinancialForecastingContext()
        {
            Database.SetInitializer(new FinancialForecastingInitializer());
        }

        public FinancialForecastingContext()
            : base("FinancialForecastingDB")
        {
        }

        public DbSet<Enterprise> Enterprises { get; set; }

        public DbSet<EnterpriseIndex> EnterpriseIndices { get; set; }
    }
}