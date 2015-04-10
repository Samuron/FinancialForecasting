using System.Data.Entity;
using FinancialForecasting.Data.Entities;

namespace FinancialForecasting.Data
{
    public class FinancialForecastingContext : DbContext
    {
        public FinancialForecastingContext() : base("FinancialForecastingDB")
        {
        }

        public DbSet<Enterprise> Enterprises { get; set; }

        public DbSet<EnterpriseIndex> EnterpriseIndices { get; set; }
    }
}