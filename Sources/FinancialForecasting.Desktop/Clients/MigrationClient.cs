using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using FinancialForecasting.Migration;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Desktop.Clients
{
    public class MigrationClient : DuplexClientBase<IMigrationService>, IMigrationService
    {
        public MigrationClient(INotifyMigrationProgress listener)
            : base(new InstanceContext(listener))
        {
        }

        public IEnumerable<EnterpriseDto> GetEnterprises()
        {
            return Channel.GetEnterprises();
        }

        public IEnumerable<EnterpriseIndexDto> GetIndexes()
        {
            return Channel.GetIndexes();
        }

        public void InsertEnterprise(EnterpriseDto enterprise)
        {
            Channel.InsertEnterprise(enterprise);
        }

        public void InsertEnterpriseIndex(EnterpriseIndexDto enterpriseIndex)
        {
            Channel.InsertEnterpriseIndex(enterpriseIndex);
        }

        public void MigrateXls(Stream stream)
        {
            Channel.MigrateXls(stream);
        }

        public void MigrateXlsx(Stream stream)
        {
            Channel.MigrateXlsx(stream);
        }
    }
}