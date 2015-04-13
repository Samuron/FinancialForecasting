using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using Excel;
using FastMapper;
using FinancialForecasting.Migration.DataContracts;
using FinancialForecasting.Migration.Entities;

namespace FinancialForecasting.Migration
{
    [ServiceBehavior(UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class MigrationService : IMigrationService, IDisposable
    {
        private readonly FinancialForecastingContext _context;
        private readonly INotifyMigrationProgress _listener;

        public MigrationService(FinancialForecastingContext context)
        {
            _context = context;
            _listener = OperationContext.Current.GetCallbackChannel<INotifyMigrationProgress>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<EnterpriseDto> GetEnterprises()
        {
            return _context.Enterprises.AsNoTracking()
                .Project().To<EnterpriseDto>()
                .AsParallel().ToArray();
        }

        public IEnumerable<EnterpriseIndexDto> GetIndexes()
        {
            return _context.EnterpriseIndices.AsNoTracking()
                .Project().To<EnterpriseIndexDto>()
                .AsParallel().ToArray();
        }

        public void InsertEnterprise(EnterpriseDto enterprise)
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

        public void MigrateXls(Stream stream)
        {
            Migrate(stream, ExcelReaderFactory.CreateBinaryReader);
        }

        public void MigrateXlsx(Stream stream)
        {
            Migrate(stream, ExcelReaderFactory.CreateOpenXmlReader);
        }

        private void Migrate(Stream stream, Func<MemoryStream, IExcelDataReader> factory)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                using (var reader = factory(memoryStream))
                {
                    var maxRows = GetMaxRows(reader);
                    var currentRow = 1;
                    _listener.AcceptMaxRows(maxRows);
                    var iterator = new ExcelEnterpriseIndexEnumerator(_context);
                    foreach (var enterpriseIndex in iterator.GetEnterpriseIndices(reader))
                    {
                        _context.EnterpriseIndices.Add(enterpriseIndex);
                        _listener.AcceptCurrentRow(currentRow++);
                    }
                    _context.SaveChanges();
                    _listener.AcceptCurrentRow(currentRow);
                    _listener.MigrationFinished();
                }
            }
        }

        private static int GetMaxRows(IExcelDataReader reader)
        {
            return reader.AsDataSet().Tables[0].Rows.Count;
        }
    }
}