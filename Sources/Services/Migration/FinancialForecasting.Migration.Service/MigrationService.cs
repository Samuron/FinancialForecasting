using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FastMapper;
using FinancialForecasting.Migration.DataContracts;
using FinancialForecasting.Migration.Entities;

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

        public IEnumerable<EnterpriseIndexDto> GetIndexes()
        {
            return _context.EnterpriseIndices.Project().To<EnterpriseIndexDto>().ToList();
        }

        public void InserEnterprise(EnterpriseDto enterprise)
        {
            var enterpriseEntity = TypeAdapter.Adapt<Enterprise>(enterprise);
            _context.Enterprises.Add(enterpriseEntity);
            _context.SaveChanges();
        }

        public void InsertEnterpriseIndex(EnterpriseIndexDto enterpriseIndex)
        {
            var enterpriseIndexEntity = TypeAdapter.Adapt<EnterpriseIndex>(enterpriseIndex);
            _context.EnterpriseIndices.Add(enterpriseIndexEntity);
            _context.SaveChanges();
        }
    }
}