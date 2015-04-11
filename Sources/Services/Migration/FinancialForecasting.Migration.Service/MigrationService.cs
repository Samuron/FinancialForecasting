using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FastMapper;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Migration
{
    [ServiceBehavior(
        UseSynchronizationContext = false, 
        InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MigrationService : IMigrationService, IDisposable
    {
        private readonly FinancialForecastingContext _context;

        public MigrationService(FinancialForecastingContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<EnterpriseDto> GetEnterprises()
        {
            return _context.Enterprises.Project().To<EnterpriseDto>().ToList();
        }

        public IEnumerable<EntetpriseIndexDto> GetIndexes()
        {
            return _context.EnterpriseIndices.Project().To<EntetpriseIndexDto>().ToList();
        }
    }
}