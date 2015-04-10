using System;
using System.Collections.Generic;
using FastMapper;
using FinancialForecasting.Data;

namespace FinancialForecasting.DataAccess
{
    public class DataAccessService : IDataAccessService
    {
        public IEnumerable<EnterpriseDto> GetEntrprises()
        {
            return Perform(x => x.Enterprises.Project().To<EnterpriseDto>());
        }

        public IEnumerable<EntetpriseIndexDto> GetIndexes()
        {
            return Perform(x => x.EnterpriseIndices.Project().To<EntetpriseIndexDto>());
        }

        private static T Perform<T>(Func<FinancialForecastingContext, T> selector)
        {
            using (var context = new FinancialForecastingContext())
            {
                return selector(context);
            }
        }
    }
}