using System;
using FinancialForecasting.Migration.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialForecasting.Migration.Tests
{
    [TestClass]
    public class FinancialForecastingDataTests
    {
        [TestMethod]
        public void EnterpriseEntityIntegrationTest()
        {
            string id = string.Empty;
            using (var context = new FinancialForecastingContext())
            {
                var enterprise = new Enterprise {Id = "TestEnterprise"};
                context.Enterprises.Add(enterprise);
                id = enterprise.Id;

                Assert.IsTrue(id != string.Empty);
                context.SaveChanges();
            }

            using (var context = new FinancialForecastingContext())
            {
                var testEntity = context.Enterprises.Find(id);
                Assert.IsNotNull(testEntity);

                context.Enterprises.Remove(testEntity);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void EnterpriseIndexEntityIntegrationTest()
        {
            string id = string.Empty;
            using (var context = new FinancialForecastingContext())
            {
                var enterprise = new Enterprise { Name = "TestEnterprise" };
                context.Enterprises.Add(enterprise);
                id = enterprise.Id;

                Assert.IsTrue(id != string.Empty);
                context.SaveChanges();
            }

            using (var context = new FinancialForecastingContext())
            {
                var testEntity = context.Enterprises.Find(id);
                Assert.IsNotNull(testEntity);

                context.Enterprises.Remove(testEntity);
                context.SaveChanges();
            }
        }
    }
}