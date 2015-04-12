using System;
using System.Collections.Generic;
using System.Data;
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
        ConcurrencyMode = ConcurrencyMode.Multiple)]
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
            return _context.Enterprises.Project().To<EnterpriseDto>().ToList();
        }

        public IEnumerable<EnterpriseIndexDto> GetIndexes()
        {
            return _context.EnterpriseIndices.Project().To<EnterpriseIndexDto>().ToList();
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
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                using (var reader = ExcelReaderFactory.CreateBinaryReader(memoryStream))
                {
                    var maxRows = reader.AsDataSet().Tables[0].Rows.Count;
                    var currentRow = 1;
                    _listener.AcceptMaxRows(maxRows);
                    reader.IsFirstRowAsColumnNames = true;
                    reader.Read();
                    while (reader.Read())
                    {
                        var id = reader.GetString(7);
                        var name = reader.GetString(8);
                        var enterprise = _context.Enterprises.Find(id) ?? InsertedEnterprise(id, name);
                        _context.EnterpriseIndices.Add(ReadEnterpriseIndex(reader, enterprise));
                        _listener.AcceptCurrentRow(currentRow++);
                    }
                    _listener.AcceptCurrentRow(currentRow);
                    _context.SaveChanges();
                }
            }
        }

        public void MigrateXlsx(Stream stream)
        {
        }

        private static EnterpriseIndex ReadEnterpriseIndex(IDataRecord reader, Enterprise enterprise)
        {
            return new EnterpriseIndex
            {
                X1 = reader.GetDouble(0),
                X2 = reader.GetDouble(1),
                X3 = reader.GetDouble(2),
                X4 = reader.GetDouble(3),
                X5 = reader.GetDouble(4),
                X6 = reader.GetDouble(5),
                X7 = reader.GetDouble(6),
                Enterprise = enterprise
            };
        }

        private Enterprise InsertedEnterprise(string id, string name)
        {
            var enterprise = new Enterprise {Id = id, Name = name};
            _context.Enterprises.Add(enterprise);
            _context.SaveChanges();
            return enterprise;
        }
    }
}