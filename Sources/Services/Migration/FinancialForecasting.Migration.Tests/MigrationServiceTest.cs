using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinancialForecasting.Migration.Tests
{
    [TestClass]
    public class MigrationServiceTest
    {
    }

    public static class Extensions
    {
        public static Task<TResult> ToAsync<TResult>(this Func<TResult> func)
        {
            return Task.Run(func);
        }
    }
}