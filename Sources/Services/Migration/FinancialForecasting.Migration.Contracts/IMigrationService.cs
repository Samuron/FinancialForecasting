using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Migration
{
    [ServiceContract(CallbackContract = typeof(INotifyMigrationProgress))]
    public interface IMigrationService
    {
        [OperationContract]
        IEnumerable<EnterpriseDto> GetEnterprises();

        [OperationContract]
        IEnumerable<EnterpriseIndexDto> GetIndexes();

        [OperationContract]
        void InsertEnterprise(EnterpriseDto enterprise);

        [OperationContract]
        void InsertEnterpriseIndex(EnterpriseIndexDto enterpriseIndex);

        [OperationContract(IsOneWay = true)]
        void MigrateXls(Stream stream);

        [OperationContract(IsOneWay = true)]
        void MigrateXlsx(Stream stream);
    }
}