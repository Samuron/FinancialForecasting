using System.Collections.Generic;
using System.ServiceModel;
using FinancialForecasting.Migration.DataContracts;

namespace FinancialForecasting.Migration
{
    [ServiceContract]
    public interface IMigrationService
    {
        [OperationContract]
        IEnumerable<EnterpriseDto> GetEnterprises();

        [OperationContract]
        IEnumerable<EnterpriseIndexDto> GetIndexes();

        [OperationContract]
        void InserEnterprise(EnterpriseDto enterprise);

        [OperationContract]
        void InsertEnterpriseIndex(EnterpriseIndexDto enterpriseIndex);
    }
}