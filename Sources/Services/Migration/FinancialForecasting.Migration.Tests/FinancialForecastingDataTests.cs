using System.Linq;
using FinancialForecasting.Migration.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialForecasting.Migration.Tests
{
    [TestClass]
    public class FinancialForecastingDataTests
    {
        [TestMethod]
        public void FinancialForecastingDatabaseCreatedSuccessfully()
        {
            using (var context = new FinancialForecastingContext())
            {
                var test = context.Enterprises.ToList();
            }
        }

        [TestMethod]
        public void EnterpriseCanBeSavedInDatabase()
        {
            using (var context = new FinancialForecastingContext())
            {
                context.Enterprises.Add(new Enterprise {Name = "TestEnterprise"});
                context.SaveChanges();
            }

            using (var context = new FinancialForecastingContext())
            {
                var testEntity = context.Enterprises.FirstOrDefault(x => x.Name == "TestEnterprise");
                Assert.IsNotNull(testEntity);
                context.Enterprises.Remove(testEntity);
                context.SaveChanges();
            }
        }
    }
}